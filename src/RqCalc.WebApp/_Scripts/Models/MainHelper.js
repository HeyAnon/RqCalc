var MainHelper = {

    Slot: {

        isPrimary: function (slot) {
            return slot.extraSlot != null;
        },

        isExtra: function (slot) {
            return slot.primarySlot != null;
        },

        hasReverse: function (slot) {
            return slot.extraSlot != null || slot.primarySlot != null;
        },

        getReverse: function (slot) {
            return [slot.extraSlot, slot.primarySlot].singleOrDefault(function (s) {
                return s != null;
            });
        },

        allowSingleHand: function (equipmentSlot, $class) {
            return equipmentSlot.types.any(function (t) {

                return t.weaponInfo != null && t.weaponInfo.isSingleHand && MainHelper.Class.isSubsetOfMany($class, t.classes);
            });
        }
    },

    Class: {

        getLevelOffset: function ($class) {
            return ObjHelper.getAllParents($class).length - 1;
        },

        isAllowedSlot: function ($class, slot) {
            return slot.types.any(function (type) {
                return !type.classes.any() || MainHelper.Class.isSubsetOfMany($class, type.classes);
            });
        },

        isSubsetOf: function ($class, set) {
            return ObjHelper.getAllChildren(set).contains($class);
        },

        isSubsetOfC: function ($class, set) {
            return MainHelper.Class.isSubsetOf($class, set.class);
        },

        isSubsetOfMany: function ($class, sets) {
            return sets.any(function (set) {
                return MainHelper.Class.isSubsetOf($class, set);
            });
        },

        isSubsetOfManyC: function ($class, sets) {
            return sets.any(function (set) {
                return MainHelper.Class.isSubsetOf($class, set.class);
            });
        },

        getAuras: function ($class, level) {
            return $class.auras.filter(function (aura) {
                return aura.level <= level;
            });
        }
    },

    CharacterEquipmentIdentity: {

        getReverse: function (equipmentIdentity) {

            var reverseSlot = MainHelper.Slot.getReverse(equipmentIdentity.slot);

            return reverseSlot == null ? null : { slot: reverseSlot, index: equipmentIdentity.index };
        }
    },

    Equipment: {

        isDoubleHand: function (equipment) {

            var wi = equipment.type.weaponInfo;

            return wi != null && !wi.isSingleHand;
        },

        isAllowedC: function (equipment, $class) {

            var classes = MainHelper.Equipment.getClassConditions(equipment);

            return !classes.any() || MainHelper.Class.isSubsetOfMany($class, classes);
        },

        isAllowed: function (equipment, character, slot) {

            if (slot === undefined) {

                return equipment.level <= character.level
                    && (equipment.gender === null || equipment.gender === character.gender)
                    && MainHelper.Equipment.isAllowedC(equipment, character.class);

            } else {

                var eSlot = equipment.type.slot;

                return MainHelper.Equipment.isAllowed(equipment, character)
                    && (eSlot === slot || (character.class.allowExtraWeapon && MainHelper.Slot.isExtra(slot) && eSlot === slot.primarySlot));
            }
        },

        getClassConditions: function (equipment) {

            var conditions = [equipment.type.classes, equipment.classes].filter(function (classes) { return classes.any(); }).map(function (classes) {
                return classes.selectMany(ObjHelper.getAllChildren);
            });

            return conditions.any() ? conditions.reduce(function (v1, v2) {
                return $.unique(v1, v2);
            }).filter(function ($class) {
                return $class.maxLevel >= equipment.level;
            }) : conditions;
        },

        getStampClasses: function (equipment, $class, sharedStamp) {
            if (sharedStamp) {
                return [];
            } else if (equipment.isPersonal) {
                return ObjHelper.getAllParents($class);
            } else {
                return equipment.classes;
            }
        }
    },

    Bonus: {

        getOrderIndex: function (bonusContainer, $class) {

            return bonusContainer.orderIndex + bonusContainer.bonuses.selectMany(function (bonus) {
                return bonus.type.filterStats;
            }).sum(function (stat) {
                return MainHelper.Stat.getOrderIndex(stat, $class);
            });
        },

        evaluateTemplate: function (bonus) {

            var textVariables = $.map(bonus.variables, function (variable, index) {
                return variable > 0 && bonus.type.variables.any(function (v) {
                    return v.hasSign && v.index === index;
                }) ? ("+" + variable) : variable;
            });

            return TextTemplate.evaluate(bonus.type.template, textVariables);
        }
    },

    Buff: {

        isAllowed: function (buff, level, talents) {

            return buff.level <= level && (buff.talentCondition == null || talents.contains(buff.talentCondition));
        }
    },

    Stat: {

        getOrderIndex: function (stat, $class) {

            if (stat.type === StatTypeConst.primary.id) {
                return stat === $class.primaryStat ? stat.orderIndex : -stat.orderIndex;
            }
            else if (stat.type === StatTypeConst.energy.id) {
                return stat === $class.energyStat ? stat.orderIndex : -stat.orderIndex;
            }
            else if (stat.isMelee != null) {
                return stat.isMelee === $class.isMelee ? stat.orderIndex : -stat.orderIndex;
            }
            else {
                return stat.orderIndex;
            };
        },

        isAllowed: function (stat, $class) {

            if (stat.isMelee != null) {
                return $class.isMelee === stat.isMelee;
            }

            if (stat.type === StatTypeConst.energy.id) {
                return $class.energyStat === stat;
            }

            if (stat.type === StatTypeConst.primary.id) {
                return $class.primaryStat === stat;
            }

            return true;
        }
    },

    Card: {

        isAllowedEE: function (card, equipmentType, equipmentClass) {

            return MainHelper.Card.getAllTypes(card).contains(equipmentType)
                && MainHelper.Card.isAllowedClass(card, equipmentClass);
        },

        getAllTypes: function (card) {

            var slotTypes = card.slots.selectMany(function (slot) {
                return slot.types;
            });

            var totalTypes = slotTypes.concat(card.types);

            return totalTypes.filter(function (type) {
                return MainHelper.Card.isAllowedWeapon(card, type);
            });
        },

        isAllowedWeapon: function (card, equipmentType) {

            return (card.isSingleHandWeapon == null || card.isSingleHandWeapon === equipmentType.weaponInfo.isSingleHand)
                && (card.isMeleeWeapon == null || card.isMeleeWeapon === equipmentType.weaponInfo.isMelee);
        },

        isAllowedClass: function (card, equipmentClass) {

            return MainHelper.CardType.isAllowedClass(card.type, equipmentClass)
                && (card.minEquipmentClass == null || card.minEquipmentClass.id <= equipmentClass.id);
        }
    },

    CardType: {

        isAllowedClass: function (cardType, equipmentClass) {

            return cardType.maxEquipmentClass == null || cardType.maxEquipmentClass.id >= equipmentClass.id;
        }
    },

    Stamp: {

        getOrderIndex: function (stampVariant, $class) {

            return stampVariant.stamp.orderIndex + stampVariant.bonuses.selectMany(function (bonus) {
                return bonus.type.filterStats;
            }).sum(function (stat) {
                return MainHelper.Stat.getOrderIndex(stat, $class);
            });
        },

        getByColor: function (stamp, color) {

            return stamp.variants.single(function (variant) {
                return variant.color === color;
            });
        }
    },

    CharacterSource: {

        getCardBuffs: function (characterSource) {

            return characterSource.equipments
                .map(function (e) {
                    return e.data();
                }).filter(function (data) {
                    return data != null;
                }).selectMany(function (data) {
                    return data.cards;
                }).map(function (cardModel) {
                    return cardModel.card();
                }).filter(function (card) {
                    return card != null;
                }).selectMany(function (card) {
                    return card.buffs;
                }).distinct();
        },

        getStampBuffs: function (characterSource) {

            return characterSource.equipments
                .map(function (e) {
                    return e.data();
                }).filter(function (data) {
                    return data != null && data.stampVariant != null;
                }).map(function (data) {
                    return data.stampVariant;
                }).selectMany(function (stampVariant) {
                    return stampVariant.stamp.buffs;
                }).distinct();
        }
    },

    Settings: {

        getMaxCardCount: function (settings, slot) {

            if (slot.isWeapon) {
                return settings.weaponCardCount;
            }
            else if (slot.isWeapon == null) {
                return 0;
            } else {
                return settings.equipmentCardCount;
            }
        }
    },

    TextTemplate: {

        evaluateMessage: function (textTemplate, evaluateStats) {

            var args = textTemplate.variables.map(function ($var) {

                var statValue = evaluateStats.getValueByKey($var.type);

                return Math.round($var.value * statValue);
            });

            return TextTemplate.evaluate(textTemplate.message, args);
        },

        getGuildTalentDescription: function (talent, points) {

            return {
                body: MainHelper.TextTemplate.getGuildTalentTextTemplate(talent, points),

                buffs: []
            }
        },

        getGuildTalentTextTemplate: function (talent, points) {

            return {

                message: talent.descriptionTemplate,

                variables: talent.variables.filter(function (v) {
                    return v.points === points;
                }).map(function (v) {
                    return {
                        index: v.index,
                        type: TextTemplateVariableType.const,
                        value: v.value
                    }
                })
            }
        }
    },

    CollectedItem: {

        isAllowed: function (collectedItem, gender) {

            return collectedItem.gender === null || collectedItem.gender === gender;
        }
    }
};