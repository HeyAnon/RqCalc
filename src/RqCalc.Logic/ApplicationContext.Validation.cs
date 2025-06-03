using System.ComponentModel.DataAnnotations;
using Framework.Core;
using RqCalc.Domain._Extensions;
using RqCalc.Domain.Model;
using RqCalc.Domain.Persistent.GuildTalent;
using RqCalc.Logic._Extensions;

namespace RqCalc.Logic;

public partial class ApplicationContext
{
    public void Validate(ICharacterSource character)
    {
        if (character == null) throw new ArgumentNullException(nameof(character));

        if (character.State == null)
        {
            throw new ValidationException("State not initialized");
        }

        if (character.Gender == null)
        {
            throw new ValidationException("Gender not initialized");
        }

        this.Validate((ITalentBuildSource)character);

        this.Validate((IGuildTalentBuildSource)character);

        character.EditStats.Keys.GetMergeResult(this.GetEditStats(character.Class)).Pipe(mergeResult =>
        {
            if (!mergeResult.IsEmpty)
            {
                var unexpectedStatsStr = mergeResult.RemovingItems.Join(", ", st => st.Name);

                var notFoundStatsStr = mergeResult.AddingItems.Join(", ", st => st.Name);

                throw new ValidationException($"Invalid stats. Unexpected stats: {unexpectedStatsStr} | NotFoundStats: {notFoundStatsStr}");
            }
        });

        character.EditStats.Where(pair => pair.Value < 1 || pair.Value > this.Settings.MaxStatCount)
            .ToList()
            .Pipe(outOfRangeStats =>
            {
                if (outOfRangeStats.Any())
                {
                    var outOfRangeStatsStr = outOfRangeStats.Join(", ", pair => $"{pair.Key.Name} ({pair.Value})");

                    throw new ValidationException($"Invalid stats. Out of range stats: {outOfRangeStatsStr}");
                }
            });

        this.GetFreeStats(character).Pipe(freeStats =>
        {
            if (freeStats < 0)
            {
                throw new ValidationException($"Invalid stats. Overflow usage stats: {-freeStats}");
            }
        });

        foreach (var equipmentPair in character.Equipments)
        {
            var currentSlot = equipmentPair.Key.Slot;
            var currentIndex = equipmentPair.Key.Index;


            var charEquipment = equipmentPair.Value;


            var equipment = charEquipment.Equipment;

            var baseSlot = equipment.Type.Slot;


            if (!charEquipment.Active && !equipment.IsActivate())
            {
                throw new ValidationException($"Invalid Equipment ({equipment.Name}) Can't be deactivated.");
            }

            if (currentIndex < 0 || currentIndex > currentSlot.Count)
            {
                throw new ValidationException($"Invalid Equipment ({equipment.Name}) Level: {currentIndex}");
            }

            var isExtraWeapon = baseSlot.IsWeapon == true && currentSlot.IsExtraSlot();

            if (isExtraWeapon)
            {
                if (!character.Class.AllowExtraWeapon)
                {
                    throw new ValidationException($"Invalid Equipment ({equipment.Name}). Extra weapon not allowed ");
                }

                character.Equipments.GetValueOrDefault(this._primaryWeaponSlot, 0).Maybe(equipmentData =>
                {
                    var primaryWeapon = equipmentData.Equipment;

                    if (primaryWeapon.Type != equipment.Type)
                    {
                        throw new ValidationException($"Invalid Equipment ({equipment.Name}). Unsync extra weapon type: {primaryWeapon.Type.Name}, {equipment.Type.Name}");
                    }
                });
            }

            if (currentSlot.IsPrimarySlot() && equipment.IsDoubleHand())
            {
                //if (currentSlot != this._primaryWeaponSlot)
                //{
                //    throw new ValidationException("Invalid Equipment ({0}) Slot: {1}. Expected: ", equipment.Name, currentSlot.Name);
                //}

                var extraCharacterEquipments =
                    character.Equipments.GetValueOrDefault(this._primaryWeaponSlot.ExtraSlot, 0);

                if (extraCharacterEquipments != null)
                {
                    throw new ValidationException($"Invalid Equipment ({extraCharacterEquipments.Equipment.Name}) ExtraSlot.");
                }
            }

            if (charEquipment.Equipment.Type.Slot != (isExtraWeapon ? this._primaryWeaponSlot : currentSlot))
            {
                throw new ValidationException($"Invalid Equipment ({equipment.Name}) Slot: {currentSlot.Name}");
            }

            //if (baseSlot.IsWeapon != null && currentSlot == this._primaryWeaponSlot.ExtraSlot && !character.Class.AllowExtraWeapon)
            //{
            //    throw new ValidationException("Invalid Equipment ({0}) ExtraSlot.", equipment.Name);
            //}

            if (equipment.Gender != null && equipment.Gender != character.Gender)
            {
                throw new ValidationException($"Invalid Equipment ({equipment.Name}) Gender: {equipment.Gender.Name}");
            }

            if (equipment.Level > character.Level)
            {
                throw new ValidationException($"Invalid Equipment ({equipment.Name}) Level: {equipment.Level}");
            }

            if (!equipment.IsAllowed(character.Class))
            {
                throw new ValidationException($"Invalid Equipment ({equipment.Name}) Class: {equipment.Conditions.Join(", ", c => c.Class.Name)}");
            }

            if (baseSlot.IsWeapon == null)
            {
                if (charEquipment.Upgrade != 0)
                {
                    throw new ValidationException($"Invalid Equipment ({equipment.Name}). Upgrade not allowed for slot: {currentSlot.Name}");
                }

                if (charEquipment.StampVariant != null)
                {
                    throw new ValidationException($"Invalid Equipment ({equipment.Name}). Stamp not allowed for slot: {currentSlot.Name}");
                }

                if (charEquipment.Cards.Any())
                {
                    throw new ValidationException($"Invalid Equipment ({equipment.Name}). Card not allowed for slot: {currentSlot.Name}");
                }
            }
            else
            {
                var equipmentClass = this.GetEquipmentClass(charEquipment.Equipment);

                var maxUpgradeLevel = equipmentClass.MaxUpgradeLevel ?? this.Settings.MaxUpgradeLevel;

                if (charEquipment.Upgrade < 0 || charEquipment.Upgrade > maxUpgradeLevel)
                {
                    throw new ValidationException($"Invalid Equipment ({equipment.Name}) Upgrade: {charEquipment.Upgrade} | Max: {maxUpgradeLevel}");
                }

                if (charEquipment.StampVariant != null)
                {
                    var stamp = charEquipment.StampVariant.Stamp;

                    if (stamp.Equipments.Any() && !stamp.Equipments.Select(e => e.Type).Contains(equipment.Type))
                    {
                        throw new ValidationException(
                            "Invalid Equipment ({0}). Stamp ({1}) not allowed for type: {2}", equipment.Name,
                            charEquipment.StampVariant.Stamp.Name, equipment.Type.Name);
                    }
                }

                if (charEquipment.Cards.Count > this.Settings.GetMaxCardCount(baseSlot))
                {
                    throw new ValidationException("Invalid Equipment ({0}). Card count: {1}", equipment.Name, charEquipment.Cards.Count);
                }

                charEquipment.Cards.Foreach((card, index) =>
                {
                    if (card != null)
                    {
                        if (card.Type.Element != null && index != 0)
                        {
                            throw new ValidationException("Invalid Equipment ({0}). Element {1} card must be firts", equipment.Name, card.Name);
                        }

                        if (!card.IsAllowed(equipment.Type, this.LastVersion, equipmentClass))
                        {
                            throw new ValidationException("Invalid Equipment ({0}). Card slot: {1}", equipment.Name, card.Name);
                        }
                    }
                });


                if (equipment.PrimaryCard != null && charEquipment.Cards.ElementAt(0) != equipment.PrimaryCard)
                {
                    throw new ValidationException("Invalid Equipment ({0}) Primary Card: {1}", equipment.Name, charEquipment.Cards[0]);
                }
            }
        }

        if (character.Aura != null)
        {
            if (!character.Class.IsSubsetOf(character.Aura))
            {
                throw new ValidationException("Invalid Aura ({0}) Class: {1}", character.Aura.Name,
                    character.Aura.Class.Name);
            }

            if (character.Aura.Level > character.Level)
            {
                throw new ValidationException("Invalid Aura ({0}) Level: {1}", character.Aura.Name,
                    character.Aura.Level);
            }
        }

        if (character.SharedAuras.Count > this.Settings.MaxPartySize - 1)
        {
            throw new ValidationException("Invalid SharedAuras Count: {0}", character.SharedAuras.Count);
        }

        character.Consumables.GetDuplicates().ToList().Pipe(consumableDuplicates =>
        {
            if (consumableDuplicates.Any())
            {
                throw new ValidationException("Invalid Consumables. Duplicate elements: {0}",
                    consumableDuplicates.Join(", ", c => c.Name));
            }
        });

        character.Buffs.Keys.GetDuplicates().ToList().Pipe(buffDuplicates =>
        {
            if (buffDuplicates.Any())
            {
                throw new ValidationException("Invalid Buffs. Duplicate elements: {0}",
                    buffDuplicates.Join(", ", b => b.Name));
            }
        });

        character.Talents.GetDuplicates().ToList().Pipe(talentDuplicates =>
        {
            if (talentDuplicates.Any())
            {
                throw new ValidationException("Invalid talents. Duplicate elements: {0}",
                    talentDuplicates.Join(", ", t => t.Name));
            }
        });

        foreach (var pair in character.Buffs)
        {
            var buff = pair.Key;

            if (buff.Class != null)
            {
                if (buff.Level > character.Level)
                {
                    throw new ValidationException("Invalid Buff ({0}) Level: {1}", buff.Name, buff.Level);
                }

                if (!character.Class.IsSubsetOf(buff.Class))
                {
                    throw new ValidationException("Invalid Buff ({0}) Class: {1}", buff.Name, buff.Class.Name);
                }

                if (pair.Value < 0 || pair.Value > buff.MaxStackCount)
                {
                    throw new ValidationException("Invalid Buff \"{0}\". Out of range stacks: {1}", buff.Name,
                        pair.Value);
                }

                if (buff.TalentCondition.Maybe(t => !character.Talents.Contains(t)))
                {
                    throw new ValidationException("Talent \"{0}\" for buff \"{1}\" not found",
                        buff.TalentCondition.Name, buff.Name);
                }
            }
            else if (buff.Card != null)
            {
                if (!character.GetCardBuffs().Contains(buff))
                {
                    throw new ValidationException("Card for Buff \"{0}\" not found", buff.Name);
                }
            }
            else if (buff.Stamp != null)
            {
                if (!character.GetStampBuffs().Contains(buff))
                {
                    throw new ValidationException("Stamp for Buff \"{0}\" not found", buff.Name);
                }
            }
        }

        character.CollectedItems.GetDuplicates().ToList().Pipe(collectedItemDuplicates =>
        {
            if (collectedItemDuplicates.Any())
            {
                throw new ValidationException($"Invalid Collected Items. Duplicate elements: {collectedItemDuplicates.Join(", ", item => item.Name)}");
            }
        });

        foreach (var collectedItem in character.CollectedItems)
        {
            if (!collectedItem.IsAllowed(character.Gender, this.LastVersion))
            {
                throw new ValidationException($"Invalid CollectedItem \"{collectedItem.Name}\"");
            }
        }
    }

    public void Validate(ITalentBuildSource character)
    {
        if (character == null) throw new ArgumentNullException(nameof(character));

        if (character.Level < 1 || character.Level > (character.Class.Specialization.MaxLevel ?? this.LastVersion.MaxLevel))
        {
            throw new ValidationException("Invalid Level. Out of range: {0}", character.Level);
        }

        character.Talents.GroupBy(tal => tal.Branch).ToList().Pipe(talentBranchGroups =>
        {
            foreach (var talentBranchGroup in talentBranchGroups)
            {
                var branch = talentBranchGroup.Key;

                if (!character.Class.IsSubsetOf(branch))
                {
                    throw new ValidationException("Invalid TalentBrunch \"{0}\" Class: {1}", branch.Name,
                        branch.Class);
                }

                foreach (var pair in branch.Talents.GroupBy(tal => tal.HIndex).OrderBy(g => g.Key)
                             .Zip(
                                 talentBranchGroup.GroupBy(tal => tal.HIndex)
                                     .OrderBy(g => g.Key)
                                     .Select(instance => instance.SingleOrDefault(dup => new Exception(
                                         $"To many talents ({dup.Join(", ", t => t.Name)}) in one vertical set {instance.Key}"))),

                                 (defintion, talent) => new { Defintion = defintion, Talent = talent }))
                {
                    var definition = pair.Defintion;

                    var talent = pair.Talent;

                    if (!definition.Contains(talent))
                    {
                        throw new ValidationException("Invalid Talent \"{0}\" Position", talent.Name);
                    }
                }
            }
        });


        this.GetFreeTalents(character).Pipe(freeTalents =>
        {
            if (freeTalents < 0)
            {
                throw new ValidationException("Invalid talents. Overflow usage talents: {0}", -freeTalents);
            }
        });
    }

    public void Validate(IGuildTalentBuildSource character)
    {
        if (character == null) throw new ArgumentNullException(nameof(character));

        var initializedTalents = character.GuildTalents.Where(pair => pair.Value != 0).ToList();

        initializedTalents.Select(t => t.Key).GetDuplicates().ToList().Pipe(guildBonusDuplicates =>
        {
            if (guildBonusDuplicates.Any())
            {
                throw new ValidationException("Invalid Guild Talents. Duplicate elements: {0}",
                    guildBonusDuplicates.Join(", ", gb => gb.Name));
            }
        });

        initializedTalents.GroupBy(pair => pair.Key.Branch, pair => pair.Key)
            .Where(g => g.Count() > 1).Foreach(overflowGroup => throw new ValidationException($"More one guild talent ({overflowGroup.Join(", ")}) in branch \"{overflowGroup.Key}\""));

        var missedBranches = this.DataSource.GetFullList<IGuildTalentBranch>().OrderById().Take(initializedTalents.Count).Except(initializedTalents.Select(t => t.Key.Branch)).ToList();

        if (missedBranches.Any())
        {
            throw new ValidationException($"Missed Talent in Branches: {missedBranches.Join(", ")}");
        }

        foreach (var pair in initializedTalents.OrderBy(tal => tal.Key.Branch.Id).Select((pair, index) => new { Talent = pair.Key, Points = pair.Value, IsLast = index == initializedTalents.Count - 1 }))
        {
            if (pair.Points <= 0 || pair.Points > pair.Talent.Branch.MaxPoints)
            {
                throw new ValidationException($"Invalid point count ({pair.Points}) in guild talent: \"{pair.Talent}\"");
            }

            if (!pair.IsLast && pair.Points != pair.Talent.Branch.MaxPoints)
            {
                throw new ValidationException($"Points ({pair.Points}) in guild talent \"{pair.Talent}\" must be maximized ({pair.Talent.Branch.MaxPoints})");
            }
        }
    }
}