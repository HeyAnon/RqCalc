using System.ComponentModel.DataAnnotations;

using Framework.Core;
using RqCalc.Domain;
using RqCalc.Domain._Extensions;
using RqCalc.Model;
using RqCalc.Model._Extensions;

namespace RqCalc.Application;

public class CharacterValidator(
    ApplicationSettings settings,
    IVersion lastVersion,
    IStatService statService,
    IFreeStatCalculator freeStatCalculator,
    IValidator<ITalentBuildSource> talentValidator,
    IValidator<IGuildTalentBuildSource> guildTalentValidator,
    IEquipmentSlotService equipmentSlotService,
    IEquipmentService equipmentService) : IValidator<ICharacterSource>
{
    public void Validate(ICharacterSource character)
    {
        talentValidator.Validate(character);

        guildTalentValidator.Validate(character);

        character.EditStats.Keys.GetMergeResult(statService.GetEditStats(character.Class)).Pipe(mergeResult =>
        {
            if (!mergeResult.IsEmpty)
            {
                var unexpectedStatsStr = mergeResult.RemovingItems.Join(", ", st => st.Name);

                var notFoundStatsStr = mergeResult.AddingItems.Join(", ", st => st.Name);

                throw new ValidationException($"Invalid stats. Unexpected stats: {unexpectedStatsStr} | NotFoundStats: {notFoundStatsStr}");
            }
        });

        character.EditStats.Where(pair => pair.Value < 1 || pair.Value > settings.MaxStatCount)
            .ToList()
            .Pipe(outOfRangeStats =>
            {
                if (outOfRangeStats.Any())
                {
                    var outOfRangeStatsStr = outOfRangeStats.Join(", ", pair => $"{pair.Key.Name} ({pair.Value})");

                    throw new ValidationException($"Invalid stats. Out of range stats: {outOfRangeStatsStr}");
                }
            });

        freeStatCalculator.GetFreeStats(character).Pipe(freeStats =>
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

                character.Equipments.GetValueOrDefault(equipmentSlotService.PrimaryWeaponSlot, 0).Maybe(equipmentData =>
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
                    character.Equipments.GetValueOrDefault(equipmentSlotService.PrimaryWeaponSlot.ExtraSlot!, 0);

                if (extraCharacterEquipments != null)
                {
                    throw new ValidationException($"Invalid Equipment ({extraCharacterEquipments.Equipment.Name}) ExtraSlot.");
                }
            }

            if (charEquipment.Equipment.Type.Slot != (isExtraWeapon ? equipmentSlotService.PrimaryWeaponSlot : currentSlot))
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
                var equipmentClass = equipmentService.GetEquipmentClass(charEquipment.Equipment);

                var maxUpgradeLevel = equipmentClass.MaxUpgradeLevel ?? settings.MaxUpgradeLevel;

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
                            $"Invalid Equipment ({equipment.Name}). Stamp ({charEquipment.StampVariant.Stamp.Name}) not allowed for type: {equipment.Type.Name}");
                    }
                }

                if (charEquipment.Cards.Count > equipmentSlotService.GetMaxCardCount(baseSlot))
                {
                    throw new ValidationException($"Invalid Equipment ({equipment.Name}). Card count: {charEquipment.Cards.Count}");
                }

                charEquipment.Cards.Foreach((card, index) =>
                {
                    if (card != null)
                    {
                        if (card.Type.Element != null && index != 0)
                        {
                            throw new ValidationException($"Invalid Equipment ({equipment.Name}). Element {card.Name} card must be firts");
                        }

                        if (!card.IsAllowed(equipment.Type, lastVersion, equipmentClass))
                        {
                            throw new ValidationException($"Invalid Equipment ({equipment.Name}). Card slot: {card.Name}");
                        }
                    }
                });


                if (equipment.PrimaryCard != null && charEquipment.Cards.ElementAt(0) != equipment.PrimaryCard)
                {
                    throw new ValidationException($"Invalid Equipment ({equipment.Name}) Primary Card: {charEquipment.Cards[0]}");
                }
            }
        }

        if (character.Aura != null)
        {
            if (!character.Class.IsSubsetOf(character.Aura.Class))
            {
                throw new ValidationException($"Invalid Aura ({character.Aura.Name}) Class: {character.Aura.Class.Name}");
            }

            if (character.Aura.Level > character.Level)
            {
                throw new ValidationException($"Invalid Aura ({character.Aura.Name}) Level: {character.Aura.Level}");
            }
        }

        if (character.SharedAuras.Count > settings.MaxPartySize - 1)
        {
            throw new ValidationException($"Invalid SharedAuras Count: {character.SharedAuras.Count}");
        }

        character.Consumables.GetDuplicates().ToList().Pipe(consumableDuplicates =>
        {
            if (consumableDuplicates.Any())
            {
                throw new ValidationException($"Invalid Consumables. Duplicate elements: {consumableDuplicates.Join(", ", c => c.Name)}");
            }
        });

        character.Buffs.Keys.GetDuplicates().ToList().Pipe(buffDuplicates =>
        {
            if (buffDuplicates.Any())
            {
                throw new ValidationException($"Invalid Buffs. Duplicate elements: {buffDuplicates.Join(", ", b => b.Name)}");
            }
        });

        character.Talents.GetDuplicates().ToList().Pipe(talentDuplicates =>
        {
            if (talentDuplicates.Any())
            {
                throw new ValidationException($"Invalid talents. Duplicate elements: {talentDuplicates.Join(", ", t => t.Name)}");
            }
        });

        foreach (var pair in character.Buffs)
        {
            var buff = pair.Key;

            if (buff.Class != null)
            {
                if (buff.Level > character.Level)
                {
                    throw new ValidationException($"Invalid Buff ({buff.Name}) Level: {buff.Level}");
                }

                if (!character.Class.IsSubsetOf(buff.Class))
                {
                    throw new ValidationException($"Invalid Buff ({buff.Name}) Class: {buff.Class.Name}");
                }

                if (pair.Value < 0 || pair.Value > buff.MaxStackCount)
                {
                    throw new ValidationException($"Invalid Buff \"{buff.Name}\". Out of range stacks: {pair.Value}");
                }

                if (buff.TalentCondition != null && !character.Talents.Contains(buff.TalentCondition))
                {
                    throw new ValidationException($"Talent \"{buff.TalentCondition.Name}\" for buff \"{buff.Name}\" not found");
                }
            }
            else if (buff.Card != null)
            {
                if (!character.GetCardBuffs().Contains(buff))
                {
                    throw new ValidationException($"Card for Buff \"{buff.Name}\" not found");
                }
            }
            else if (buff.Stamp != null)
            {
                if (!character.GetStampBuffs().Contains(buff))
                {
                    throw new ValidationException($"Stamp for Buff \"{buff.Name}\" not found");
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
            if (!collectedItem.IsAllowed(character.Gender, lastVersion))
            {
                throw new ValidationException($"Invalid CollectedItem \"{collectedItem.Name}\"");
            }
        }
    }
}