function CharacterApplicationContext(data, facade, dialogManager) {

    var context = this;

    $.extend(context, new TalentBuildApplicationContextBase(context, data, facade));
    $.extend(context, new GuildTalentBuildApplicationContextBase(context, data, facade));

    context.dialogManager = dialogManager;
    //-------------------------------
    context.genders = context.genders.map(function (g) {
        return $.extend(g, {

            image: facade.getImageUrl("Gender", g.id)
        });
    });
    //-------------------------------
    context.elements = context.elements.map(function (element) {
        return $.extend(element, {

            image: facade.getImageUrl("Element", element.id)
        });
    });
    //-------------------------------
    context.getStat = function (statId) {
        return context.stats.getById(statId);
    };
    //-------------------------------
    context.stats = context.stats.map(function (s) {
        return $.extend(s, {

            image: facade.getImageUrl("Stat", s.id),

            element: context.elements.getByIdOrDefault(s.element.id),

            race: context.races.getByIdOrDefault(s.race.id),

            restoreStats: s.restoreStats.map(function (pair) {
                return { key: pair.key, value: context.stats.getByIdOrDefault(pair.value.id) };
            })
        });
    });
    //-------------------------------
    context.getBonusesBody = function (bonuses) {

        return bonuses.map(function (bonus) {
            return $.extend(bonus, {

                type: context.bonusTypes.getById(bonus.type.id)
            });
        }).map(function (bonus) {
            return $.extend(bonus, {

                text: MainHelper.Bonus.evaluateTemplate(bonus),

                type: $.extend(bonus.type, {

                    filterStats: bonus.type.filterStats.map(function (statIdentity) {
                        return context.stats.getById(statIdentity.id);
                    })
                })
            });
        });
    };
    //-------------------------------
    context.getBonuses = function (bonusContainer) {

        return context.getBonusesBody(bonusContainer.bonuses);
    };
    //-------------------------------
    context.classes = context.classes.map(function ($class) {

        var talents = $class.talentBranches.selectMany(function (talentBranch) {
            return talentBranch.talents;
        });

        return $.extend($class, {

            auras: $class.auras.map(function (aura) {
                return $.extend(aura, {
                    image: facade.getImageUrl("Aura", aura.id),

                    bonuses: context.getBonuses(aura),

                    sharedBonusesWithoutTalents: context.getBonusesBody(aura.sharedBonusesWithoutTalents),

                    sharedBonusesWithTalents: context.getBonusesBody(aura.sharedBonusesWithTalents)
                });
            }),

            buffs: $class.buffs.map(function (buff) {
                return $.extend(buff, {
                    "class": $class,
                    image: facade.getImageUrl("Buff", buff.id),
                    talentCondition: talents.getByIdOrDefault(buff.talentCondition.id)
                });
            }),

            primaryStat: context.getStat($class.primaryStat.id),

            energyStat: context.getStat($class.energyStat.id)
        });
    }).map(function ($class) {

        var parents = ObjHelper.getAllParents($class).reverseC();

        return $.extend($class, {

            auras: parents.selectMany(function (c) {
                return c.auras;
            }),

            baseAuras: $class.auras,

            buffs: parents.selectMany(function (c) {
                return c.buffs;
            })
        });
    });
    //-------------------------------
    context.slots = context.slots.map(function (s) {
        return $.extend(s, {

            image: facade.getImageUrl("EquipmentSlot", s.id),

            extraSlot: context.slots.getByIdOrDefault(s.extraSlot.id),

            primarySlot: context.slots.getByIdOrDefault(s.primarySlot.id),

            types: s.types.map(function (t) {

                return $.extend(t, {

                    slot: s,

                    classes: t.classes.map(function (classIdentity) {

                        return context.getClass(classIdentity.id);
                    })
                });
            })
        });
    });
    //-------------------------------
    var equipmentTypes = context.slots.selectMany(function (s) {
        return s.types;
    });
    //-------------------------------
    context.stampColors = context.stampColors.map(function (sc) {
        return $.extend(sc, {

            image: facade.getImageUrl("StampColor", sc.id),

            bigImage: facade.getImageUrl("BigStampColor", sc.id)
        });
    });
    //-------------------------------
    context.stamps = context.stamps.map(function (stamp) {
        return $.extend(stamp, {
            variants: stamp.variants.map(function (stampVariant) {

                return $.extend(stampVariant, {
                    stamp: stamp,
                    color: context.stampColors.getById(stampVariant.color.id),
                    bonuses: context.getBonuses(stampVariant)
                });
            }),

            equipmentTypes: stamp.equipmentTypes.map(function (equipmentTypeIdentity) {

                return equipmentTypes.getById(equipmentTypeIdentity.id);
            }),

            buffs: stamp.buffs.map(function (buff) {
                return $.extend(buff, {
                    image: facade.getImageUrl("Buff", buff.id),
                    stamp: stamp
                });
            })
        });
    });
    //-------------------------------
    context.cardTypes = context.cardTypes.map(function (ct) {
        return $.extend(ct, {
            image: facade.getImageUrl("CardType", ct.id),

            element: context.elements.getByIdOrDefault(ct.element.id),

            toolTipImage: ct.hasToolTipImage ? facade.getImageUrl("CardTypeToolTip", ct.id) : null,

            maxEquipmentClass: context.equipmentClasses.getByIdOrDefault(ct.maxEquipmentClass.id)
        });
    });
    //-------------------------------
    context.cards = context.cards.map(function (card) {
        return $.extend(card, {
            slots: card.slots.map(function (slotIdentity) {

                return context.slots.getById(slotIdentity.id);
            }),

            types: card.types.map(function (typeIdentity) {

                return equipmentTypes.getById(typeIdentity.id);
            }),

            type: context.cardTypes.getById(card.type.id),

            bonuses: context.getBonuses(card),

            buffs: card.buffs.map(function (buff) {
                return $.extend(buff, {
                    image: facade.getImageUrl("Buff", buff.id),
                    card: card
                });
            }),

            groupValue: card.group == null ? null : card.group.value,

            minEquipmentClass: context.equipmentClasses.getByIdOrDefault(card.minEquipmentClass.id)
        });
    });
    //-------------------------------
    context.equipments = context.equipments.map(function (equipment) {
        return $.extend(equipment, {

            image: facade.getImageUrl("Equipment", equipment.id),

            costumeImage: equipment.isCostume ? facade.getImageUrl("Costume", equipment.id) : null,

            type: equipmentTypes.getById(equipment.type.id),

            gender: context.genders.getByIdOrDefault(equipment.gender.id),

            "class": context.equipmentClasses.getById(equipment.class.id),

            classes: equipment.classes.map(function (classIdentity) {

                return context.getClass(classIdentity.id);
            }),

            bonuses: context.getBonuses(equipment),

            primaryCard: context.cards.getByIdOrDefault(equipment.primaryCard.id)
        });
    });
    //-------------------------------
    context.elixirs = context.elixirs.map(function (elixir) {
        return $.extend(elixir, {
            image: facade.getImageUrl("Elixir", elixir.id),
            bonuses: context.getBonuses(elixir)
        });
    });
    //-------------------------------
    context.consumables = context.consumables.map(function (consumable) {
        return $.extend(consumable, {
            image: facade.getImageUrl("Consumable", consumable.id),
            bonuses: context.getBonuses(consumable)
        });
    });
    //-------------------------------
    context.getDefaultCharacter = function () {
        return ObjHelper.clone(context.defaultCharacter);
    };
    //-------------------------------
    {
        var classPrimaryStats = context.classes.map(function ($class) {
            return $class.primaryStat;
        });

        context.notPrimaryEditStats = context.stats.except(classPrimaryStats).filter(function (stat) {
            return stat.isEditable;
        });
    }

    {
        var classStats = context.classes.selectMany(function ($class) {
            return $.merge([$class.primaryStat, $class.energyStat], $class.energyStat.restoreStats.getValues());
        });

        context.staticDisplayStats = context.stats.except(classStats);
    }
    //-------------------------------
    context.getSharedAuras = function () {

        return context.classes.selectMany(function ($class) {
            return $class.baseAuras;
        }).filter(function (aura) {
            return aura.sharedBonusesWithoutTalents.any() || aura.sharedBonusesWithTalents.any();
        }).orderBy(function (aura) {
            return aura.name;
        });
    }
    //-------------------------------
    context.sharedBuffs = context.sharedBuffs.map(function (buff) {
        return $.extend(buff, {
            image: facade.getImageUrl("Buff", buff.id)
        });
    });
    //-------------------------------
    context.equipmentElixirs = context.equipmentElixirs.map(function (ee) {
        return $.extend(ee, {

            image: facade.getImageUrl("EquipmentElixir", ee.id),

            slots: ee.slots.map(function (slotIdentity) {
                return context.slots.getById(slotIdentity.id);
            }),

            bonuses: context.getBonuses(ee)
        });
    });
    //-------------------------------
    context.collectedGroups = context.collectedGroups.map(function (group) {

        return $.extend(group, {

            statistic: context.collectedStatistics.getByIdOrDefault(group.statistic.id),

            items: group.items.map(function (collectedItem) {
                return $.extend(collectedItem, {

                    image: facade.getImageUrl("CollectedItem", collectedItem.id),

                    gender: context.genders.getByIdOrDefault(collectedItem.gender.id),

                    bonuses: context.getBonuses(collectedItem),

                    group: group
                });
            })
        });
    });

    context.getCollectedItemById = function (collectedItemId) {
        return context.collectedGroups.selectMany(function (group) {
            return group.items;
        }).single(function (collectedItem) {
            return collectedItem.id === collectedItemId;
        });
    };

    //-------------------------------
    context.buffs = [

        context.classes.selectMany(function ($class) {
            return $class.buffs;
        }),

        context.sharedBuffs,

        context.cards.selectMany(function ($class) {
            return $class.buffs;
        }),

        context.stamps.selectMany(function ($class) {
            return $class.buffs;
        })

    ].selectMany().distinct();

    //-------------------------------
    context.getFreeStats = function (character) {

        var allAvailableStats = (character.level - 1) * context.settings.statsPerLevel;

        var usedStats = character.editStats.getValues()
            .sum(function (editStat) {
                return editStat.editValue() - 1;
            });

        return allAvailableStats - usedStats;
    };
    //-------------------------------
    context.isAllowedStamp = function (stamp, equipment, currentClass) {

        if (stamp.equipmentTypes.any() && !stamp.equipmentTypes.contains(equipment.type)) {
            return false;
        }

        var equipmentClass = equipment.class;

        var filterStats = stamp.variants.selectMany(function (variant) {
            return variant.bonuses;
        }).selectMany(function (bonus) {
            return bonus.type.filterStats;
        }).distinct();

        if (filterStats.any()) {

            var currentClassAllowed = filterStats.all(function (stat) { return  MainHelper.Stat.isAllowed(stat, currentClass) });

            if (!currentClassAllowed) {

                var stampClasses = MainHelper.Equipment.getStampClasses(equipment, currentClass, equipment.type.slot.sharedStamp && equipmentClass.sharedStamp);

                if (!stampClasses.any() || stampClasses.any(function (c) {
                    return filterStats.all(function (stat) { MainHelper.Stat.isAllowed(stat, c) });
                })) {
                    return null;
                }
                else {
                    return false;
                }
            }
        }

        return true;
    }
}