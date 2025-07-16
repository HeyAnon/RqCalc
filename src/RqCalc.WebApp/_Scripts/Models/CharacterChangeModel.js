function CharacterChangeModel(context, startCharacter) {
    
    var thisModel = this;

    $.extend(thisModel, new UpdateCodeModel(thisModel, context.facade.calcCharacter, context.facade.decryptCharacter));

    thisModel.gender = ko.observable();
    thisModel.class = ko.observable();
    thisModel.level = ko.observable();
    thisModel.state = ko.observable();
    thisModel.event = ko.observable();


    thisModel.minLevel = ko.observable();
    thisModel.maxLevel = ko.observable();


    thisModel.lostControl = ko.observable();

    thisModel.freeStats = ko.observable(0);

    thisModel.characterImage = ko.observable();

    thisModel.elixir = new ActiveImageModel(context, StaticImageType.elixir, "Эликсиры", null, switchElixir);
    thisModel.consumables = new ActiveImageModel(context, StaticImageType.consumable, "Расходка", [], switchConsumables);
    thisModel.guildTalents = new ActiveImageModel(context, StaticImageType.guild, "Гильдийные таланты", [], switchGuildTalents);
    thisModel.talents = new ActiveImageModel(context, StaticImageType.talent, "Таланты", [], switchTalents);
    thisModel.aura = new ActiveImageModel(context, StaticImageType.aura, "Аура", null, switchAura);
    thisModel.sharedAuras = new ActiveImageModel(context, StaticImageType.sharedAuras, "Групповые ауры", [], null);
    thisModel.buffs = new ActiveImageModel(context, StaticImageType.buff, "Бафы", [], switchBuffs);
    thisModel.collectedItems = new ActiveImageModel(context, StaticImageType.collections, "Коллекции", [], switchCollections);

    thisModel.equipments = context.slots.selectMany(function (slot) {

        return Enumerable.range(0, slot.count).map(function (index) {

            return new EquipmentChangeModel(context, thisModel, { slot: slot, index: index });
        });
    });

    thisModel.getEquipment = function (slotId, index) {
        return thisModel.equipments.single(function (model) {
            return model.identity.slot.id === slotId && model.identity.index === index;
        });
    };

    thisModel.equipments.foreach(function (equipmentModel) {

        var reverseIdentity = MainHelper.CharacterEquipmentIdentity.getReverse(equipmentModel.identity);

        equipmentModel.reverseModel = reverseIdentity == null ? null : thisModel.getEquipment(reverseIdentity.slot.id, reverseIdentity.index);
    });

    thisModel.displayStats = $.merge([StatConst.primaryStat, StatConst.energyStat, StatConst.restoreEnergyStat, StatConst.restoreEnergyPerHitStat, StatConst.restoreEnergyPerKillStat], context.staticDisplayStats).map(function (stat) {
        return { key: stat.id, value: new DisplayStatModel(stat) };
    });

    thisModel.editStats = $.merge([StatConst.primaryStat], context.notPrimaryEditStats).map(function (stat) {
        return { key: stat.id, value: new CharacterStatEditModel(context, thisModel, thisModel.displayStats.getValueByKey(stat.id)) };
    });

    //-----------------------------------------------------------------------------------------------------------------
    //-----------------------------------------------------------------------------------------------------------------

    var displayStatNameDict = thisModel.displayStats.getValues().toDict(function (displayStat) {
        return displayStat.stat().name;
    });

    thisModel.getDisplayStat = function (statName) {
        return displayStatNameDict.getValueByKey(statName);
    };

    thisModel.getDisplayStatPair = function (statName, customDisplayName, colspan, specifications) {

        var displayModel = thisModel.getDisplayStat(statName);

        return new function () {

            var pairModel = this;

            pairModel.displayModel = displayModel;

            pairModel.name = ko.computed(function () {

                return customDisplayName == null ? pairModel.displayModel.stat().name : customDisplayName;
            });

            pairModel.colspan = colspan == null ? 1 : colspan;

            pairModel.specifications = specifications == null ? true : specifications;
        }
    };

    thisModel.getEditStat = function (statId) {
        return thisModel.editStats.getValueByKey(statId);
    };

    var editStatNameDict = thisModel.editStats.getValues().toDict(function (editStat) {
        return editStat.stat().name;
    });

    thisModel.getEditStatByName = function (statName) {
        return editStatNameDict.getValueByKey(statName);
    };

    //-----------------------------------------------------------------------------------------------------------------
    function switchElixir() {

        context.dialogManager.switchElixir(new ElixirsWindowModel(context, thisModel.class(), thisModel.elixir.value())).run(function (tryResult) {

            if (tryResult.isSuccess) {
                thisModel.elixir.value(tryResult.result);
            }

            return;
        });
    };
    //-----------------------------------------------------------------------------------------------------------------
    function switchConsumables() {

        context.dialogManager.switchConsumables(new ConsumablesWindowModel(context, thisModel.consumables.value())).run(function (tryResult) {

            if (tryResult.isSuccess) {
                thisModel.consumables.value(tryResult.result);
            }

            return;
        });
    };
    //-----------------------------------------------------------------------------------------------------------------
    function switchGuildTalents() {

        context.dialogManager.switchGuildTalents(new GuildTalentsWindowModel(context, { guildTalents: thisModel.guildTalents.value() })).run(function (tryResult) {

            if (tryResult.isSuccess) {
                thisModel.guildTalents.value(tryResult.result);
            }

            return;
        });
    };
    //-----------------------------------------------------------------------------------------------------------------
    function switchTalents() {
        
        context.dialogManager.switchTalents(new TalentsWindowModel(context, thisModel.getTemplateEvaluateStats(), thisModel.unwrap(), true)).run(function (tryResult) {

            if (tryResult.isSuccess) {
                thisModel.talents.value(tryResult.result);
            }

            return;
        });
    };
    //-----------------------------------------------------------------------------------------------------------------
    function switchAura() {

        context.dialogManager.switchAura(new AurasWindowModel(context, thisModel.class(), thisModel.aura.value(), thisModel.level(), thisModel.sharedAuras.value())).run(function (tryResult) {

            if (tryResult.isSuccess) {

                thisModel.updateOperation(function () {

                    thisModel.aura.value(tryResult.result.primaryAura);
                    thisModel.sharedAuras.value(tryResult.result.sharedAuras);
                });
            }

            return;
        });
    };
    //-----------------------------------------------------------------------------------------------------------------
    function switchBuffs() {
        
        context.dialogManager.switchBuffs(new BuffsWindowModel(context, thisModel)).run(function (tryResult) {

            if (tryResult.isSuccess) {

                thisModel.updateOperation(function () {

                    thisModel.buffs.value(tryResult.result);
                });

            }

            return;
        });
    };

    //-----------------------------------------------------------------------------------------------------------------
    function switchCollections() {

        context.dialogManager.switchCollections(new CollectionsWindowModel(context, thisModel.gender(), thisModel.collectedItems.value())).run(function (tryResult) {

            if (tryResult.isSuccess) {

                thisModel.updateOperation(function () {

                    thisModel.collectedItems.value(tryResult.result);
                });

            }

            return;
        });
    };

    //-----------------------------------------------------------------------------------------------------------------

    thisModel.resetStats = function () {

        thisModel.updateOperation(function () {

            thisModel.editStats.getValues().foreach(function (editStat) {
                editStat.editValue(1);
            });

        }, false);
    };

    //-----------------------------------------------------------------------------------------------------------------

    thisModel.getTemplateEvaluateStats = function () {

        var attack = thisModel.displayStats.getValueByKey(context.settings.attackStat.id).value();
        var defense = thisModel.displayStats.getValueByKey(context.settings.defenseStat.id).value();;

        return [{ key: TextTemplateVariableType.const,   value: 1 },
                { key: TextTemplateVariableType.attack,  value: attack },
                { key: TextTemplateVariableType.defense, value: defense }];
    }

    //-----------------------------------------------------------------------------------------------------------------

    thisModel.getEquipmentEditModel = function (model) {

        if (MainHelper.Slot.isExtra(model.identity.slot)) {

            if (model.reverseModel.isDoubleHand()) {
                return model.reverseModel;
            }

            if (!MainHelper.Slot.allowSingleHand(model.reverseModel.identity.slot, thisModel.class())) {
                return model.reverseModel;
            }
        }

        return model;
    };

    thisModel.gender.subscribe(function (_) {

        thisModel.updateOperation(function () {

            updateEquipment();
            updateCollectedItems();
        });

        return;
    });

    thisModel.class.subscribe(function (_) {

        thisModel.updateOperation(function () {

            updateCharacterClass();

            updateTalents();

            updateEquipment();
            updateFreeStats();
            updateAura();
            updateBuffs();
        });
    });

    thisModel.level.subscribe(function (_) {

        thisModel.updateOperation(function () {

            updateEquipment();
            updateFreeStats();

            updateTalents();
            updateAura();
            updateBuffs();
        });

        return;
    });

    thisModel.state.subscribe(thisModel.handleUpdate);

    thisModel.event.subscribe(thisModel.handleUpdate);

    thisModel.editStats.getValues().foreach(function (editStat) {
        editStat.editValue.subscribe(thisModel.handleUpdate);
    });

    thisModel.equipments.foreach(function (equipmentModel) {

        var prevActiveSubscribe = null;

        function subscribeActive() {

            if (prevActiveSubscribe != null) {
                prevActiveSubscribe.dispose();
                prevActiveSubscribe = null;
            }

            var data = equipmentModel.data();

            if (data != null) {

                prevActiveSubscribe = data.active.subscribe(thisModel.handleUpdate);
            }
        }

        subscribeActive();

        equipmentModel.data.subscribe(function (_) {

            subscribeActive();

            thisModel.updateOperation(function () {

                if (MainHelper.Slot.isPrimary(equipmentModel.identity.slot) && equipmentModel.isDoubleHand()) {
                    equipmentModel.reverseModel.data(null);
                }

                updateCharacterImage();

                updateBuffs();
            });
        });
    });


    thisModel.elixir.active.subscribe(thisModel.handleUpdate);
    thisModel.elixir.value.subscribe(thisModel.handleUpdate);

    thisModel.consumables.active.subscribe(thisModel.handleUpdate);
    thisModel.consumables.value.subscribe(thisModel.handleUpdate);

    thisModel.guildTalents.active.subscribe(thisModel.handleUpdate);
    thisModel.guildTalents.value.subscribe(thisModel.handleUpdate);

    thisModel.talents.active.subscribe(thisModel.handleUpdate);
    thisModel.talents.value.subscribe(function (_) {

        thisModel.updateOperation(function () {

            updateBuffs();
        });

        return;
    });

    thisModel.aura.active.subscribe(thisModel.handleUpdate);
    thisModel.aura.value.subscribe(thisModel.handleUpdate);

    thisModel.sharedAuras.value.subscribe(thisModel.handleUpdate);
    
    thisModel.buffs.active.subscribe(thisModel.handleUpdate);
    thisModel.buffs.value.subscribe(thisModel.handleUpdate);

    thisModel.collectedItems.active.subscribe(thisModel.handleUpdate);
    thisModel.collectedItems.value.subscribe(thisModel.handleUpdate);

    thisModel.lostControl.subscribe(thisModel.handleUpdate);

    //-----------------------------------------------------------------------------------------------------------------
    thisModel.maximizeLevel = function () {
        thisModel.level(thisModel.maxLevel());
    };

    //-----------------------------------------------------------------------------------------------------------------

    var primaryStat;
    var energyStat;

    //-----------------------------------------------------------------------------------------------------------------

    thisModel.setData = function (character) {

        thisModel.updateOperation(function () {
            
            var $class = context.getClass(character.class.id);
            var talents = $class.talentBranches.selectMany(function (tb) {
                return tb.talents;
            });

            thisModel.gender(context.genders.getById(character.gender.id));
            thisModel.class($class);
            thisModel.level(character.level);
            thisModel.event(context.events.getByIdOrDefault(character.event.id));
            thisModel.state(context.states.getById(character.state.id));

            thisModel.lostControl(character.lostControl);

            thisModel.equipments.foreach(function (equipmentModel) {

                var newDataPair = character.equipments.singleOrDefault(function (e) {

                    return equipmentModel.identity.slot.id === e.key.slot.id && equipmentModel.identity.index === e.key.index;
                });

                equipmentModel.setPureData(newDataPair == null ? null : normalizeData(newDataPair.value));
            });

            thisModel.elixir.value(context.elixirs.getByIdOrDefault(character.elixir.id));

            thisModel.consumables.value(character.consumables.map(function (consumable) {
                return context.consumables.getById(consumable.id);
            }));

            thisModel.guildTalents.value(character.guildTalents.map(function (guildTalent) {
                return {
                    key: context.guildTalents.getById(guildTalent.key.id),
                    value: guildTalent.value
                };
            }));

            thisModel.talents.value(character.talents.map(function (talent) {
                return talents.getById(talent.id);
            }));

            thisModel.aura.value($class.auras.getByIdOrDefault(character.aura.id));

            var allSharedAuras = context.getSharedAuras();

            thisModel.sharedAuras.value(character.sharedAuras.map(function (sharedAura) {
                return {
                    key: allSharedAuras.getById(sharedAura.key.id),
                    value: sharedAura.value
                };
            }));

            thisModel.buffs.value(character.buffs.map(function (buff) {
                return {
                    key: context.buffs.getById(buff.key.id),
                    value: buff.value
                };
            }));

            thisModel.collectedItems.value(character.collectedItems.map(function (collectedItem) {
                return context.getCollectedItemById(collectedItem.id);
            }));

            thisModel.elixir.active(character.enableElixir);
            thisModel.consumables.active(character.enableConsumables);
            thisModel.guildTalents.active(character.enableGuildTalents);
            thisModel.talents.active(character.enableTalents);
            thisModel.aura.active(character.enableAura);
            thisModel.buffs.active(character.enableBuffs);
            thisModel.collectedItems.active(character.enableCollecting);

            updateClassStats();

            thisModel.editStats.getValues().foreach(function (editModel) {

                var newValue = character.editStats.getValueByKeyId(editModel.stat().id);

                editModel.editValue(newValue);
            });

            return;

        }, false);
    };

    function normalizeData(data) {

        return $.extend(data, {
            equipment: context.equipments.getById(data.equipment.id),

            cards: data.cards.map(function (card) {
                return context.cards.getByIdOrDefault(card.id);
            }),

            stampVariant: data.stampVariant == null ? null :
                context.stamps.getById(data.stampVariant.stamp.id).variants.single(function (variant) {
                    return variant.color.id === data.stampVariant.color.id;
                }),

            elixir: context.equipmentElixirs.getByIdOrDefault(data.elixir.id)
        });
    }

    function updateFreeStats() {

        var freeStats = getFreeStats();

        while (freeStats < 0) {

            var lastNotEmptyStat = thisModel.editStats.getValues().reverse().first(function (editStat) {

                return editStat.editValue() > 1;
            });

            var delta = lastNotEmptyStat.editValue() > -freeStats ? -freeStats : lastNotEmptyStat.editValue() - 1;

            lastNotEmptyStat.editValue(lastNotEmptyStat.editValue() - delta);
            freeStats += delta;
        }

        thisModel.freeStats(getFreeStats());
    }

    function getFreeStats() {
        return context.getFreeStats(thisModel.unwrap());
    }

    function updateCharacterImage() {

        var costumeModel = thisModel.equipments.singleOrDefault(function (equipment) {
            var data = equipment.data();

            return data != null && data.equipment.isCostume;
        });

        thisModel.characterImage(costumeModel == null ? thisModel.gender().image : costumeModel.data().equipment.costumeImage);
    }

    function updateAura() {

        var avalAuras = MainHelper.Class.getAuras(thisModel.class(), thisModel.level());

        var aura = thisModel.aura.value();

        if (aura === null || !avalAuras.contains(aura)) {

            thisModel.aura.value(avalAuras.firstOrDefault());
        }
    }

    function updateBuffs() {

        var newBuffs = [getUpdatedClassBuffs(), getUpdatedSharedBuffs(), getUpdatedCardBuffs(), getUpdatedStampBuffs()].selectMany();

        thisModel.buffs.value(newBuffs);
    }

    function getUpdatedClassBuffs() {
        var level = thisModel.level();

        var talents = thisModel.talents.value();

        var newClassBuffs = thisModel.buffs.value().filter(function (buffPair) {

            var buff = buffPair.key;

            return buff.class != null && MainHelper.Class.isSubsetOf(thisModel.class(), buff.class)

                && MainHelper.Buff.isAllowed(buff, level, talents);
        });

        var autoEnabledClassBuffs = thisModel.class().buffs.filter(function (buff) {
            return !newClassBuffs.getKeys().contains(buff) && buff.autoEnabled && MainHelper.Buff.isAllowed(buff, level, talents);
        }).toDict(FuncHelper.identity, function (buff) {
            return buff.maxStackCount;
        });

        return $.merge(newClassBuffs, autoEnabledClassBuffs);
    }

    function getUpdatedSharedBuffs() {

        return thisModel.buffs.value().filter(function (buffPair) {

            var buff = buffPair.key;

            return context.sharedBuffs.contains(buff);
        });
    }

    function getUpdatedCardBuffs() {

        var cardBuffs = MainHelper.CharacterSource.getCardBuffs(thisModel);

        return thisModel.buffs.value().filter(function (buffPair) {

            var buff = buffPair.key;

            return buff.card != null && cardBuffs.contains(buff);
        });
    }

    function getUpdatedStampBuffs() {

        var stampBuffs = MainHelper.CharacterSource.getStampBuffs(thisModel);

        return thisModel.buffs.value().filter(function (buffPair) {

            var buff = buffPair.key;

            return buff.stamp != null && stampBuffs.contains(buff);
        });
    }


    function updateTalents() {

        var newTalents = thisModel.talents.value().filter(function (tal) {

            return MainHelper.Class.isSubsetOf(thisModel.class(), tal.branch.class);
        });

        thisModel.talents.value(newTalents);

        thisModel.talents.value(context.getLimitedTalents(thisModel.unwrap()));
    }

    function updateEquipment() {
        thisModel.equipments.foreach(function (equipmentModel) {

            var data = equipmentModel.data();

            if (data != null) {

                if (!MainHelper.Equipment.isAllowed(data.equipment, thisModel.unwrap(), equipmentModel.identity.slot)) {
                    equipmentModel.data(null);
                }
                else if (data.stampVariant != null && context.isAllowedStamp(data.stampVariant.stamp, data.equipment, thisModel.class()) === false) {
                    var newData = {

                        equipment: data.equipment,
                        active: data.active,
                        upgrade: data.upgrade,
                        cards: data.cards.map(function (cardModel) {
                            return cardModel.card();
                        }),
                        stampVariant: null
                    };

                    equipmentModel.setPureData(newData);
                }
            }
        });

        updateCharacterImage();
    }

    function updateCharacterClass() {

        thisModel.equipments.foreach(function (equipmentModel) {

            var isAllowed = MainHelper.Class.isAllowedSlot(thisModel.class(), equipmentModel.identity.slot)
                         || MainHelper.Slot.isExtra(equipmentModel.identity.slot);

            equipmentModel.isAllowed(isAllowed);
        });

        updateClassStats();

        thisModel.minLevel(thisModel.class().minLevel);
        thisModel.maxLevel(thisModel.class().maxLevel);

        thisModel.level(thisModel.level().minMax(thisModel.minLevel(), thisModel.maxLevel()));
    }

    function updateClassStats() {

        primaryStat = thisModel.class().primaryStat;
        energyStat = thisModel.class().energyStat;

        thisModel.getEditStat(StatConst.primaryStat.id).stat(primaryStat);

        thisModel.getDisplayStat(StatConst.primaryStat.name).stat(primaryStat);
        thisModel.getDisplayStat(StatConst.energyStat.name).stat(energyStat);
        thisModel.getDisplayStat(StatConst.restoreEnergyStat.name).stat(energyStat.restoreStats.getValueByKey(StatTypeConst.passive.id));
        thisModel.getDisplayStat(StatConst.restoreEnergyPerHitStat.name).stat(energyStat.restoreStats.getValueByKey(StatTypeConst.perHit.id));
        thisModel.getDisplayStat(StatConst.restoreEnergyPerKillStat.name).stat(energyStat.restoreStats.getValueByKey(StatTypeConst.perKill.id));
    }

    
    function updateCollectedItems() {

        var newValues = thisModel.collectedItems.value().filter(function (item) {
            return MainHelper.CollectedItem.isAllowed(item, thisModel.gender());
        });

        thisModel.collectedItems.value(newValues);
    }


    thisModel.unwrap = function() {
        return {
            "class": thisModel.class(),
            level: thisModel.level(),
            gender: thisModel.gender(),
            talents: thisModel.talents.value(),
            editStats: thisModel.editStats
        };
    };


    thisModel.recalculate = function () {

        thisModel.freeStats(context.getFreeStats(thisModel.unwrap()));

        updateCharacterImage();
        updateCharacterClass();
    };

    thisModel.getCalcData = function() {

        var elixir = thisModel.elixir.value();
        var aura = thisModel.aura.value();
        var event = thisModel.event();

        return {
            "class": { id: thisModel.class().id },

            gender: { id: thisModel.gender().id },

            level: thisModel.level(),

            state: { id: thisModel.state().id },

            event: { id: event == null ? 0 : event.id },

            editStats: thisModel.editStats.getValues()
                .map(function(model) {

                    return {
                        key: { id: model.stat().id },
                        value: model.editValue()
                    }
                }),

            equipments: thisModel.equipments.filter(function(equipmentModel) {
                    return equipmentModel.data() !== null;
                })
                .map(function(equipmentModel) {

                    var data = equipmentModel.data();

                    return {
                        key: {
                            slot: { id: equipmentModel.identity.slot.id },
                            index: equipmentModel.identity.index
                        },

                        value: {
                            equipment: {
                                id: data.equipment.id
                            },

                            cards: data.cards.map(function(cardModel) {

                                var card = cardModel.card();

                                return {
                                    id: card == null ? 0 : card.id
                                };
                            }),

                            stampVariant: data.stampVariant == null
                                ? null
                                : {
                                    stamp: {
                                        id: data.stampVariant.stamp.id
                                    },
                                    color: {
                                        id: data.stampVariant.color.id
                                    }
                                },

                            upgrade: data.upgrade,

                            active: data.active(),

                            elixir: {
                                id: data.elixir == null ? 0 : data.elixir.id
                            }
                        }
                    }
                }),

            elixir: { id: elixir == null ? 0 : elixir.id },

            consumables: thisModel.consumables.value()
                .map(function(consumable) {
                    return { id: consumable.id };
                }),

            guildTalents: thisModel.guildTalents.value()
                .map(function(guildTalent) {
                    return {
                        key: { id: guildTalent.key.id },

                        value: guildTalent.value
                    };
                }),

            talents: thisModel.talents.value()
                .map(function(talent) {
                    return { id: talent.id };
                }),

            aura: { id: aura == null ? 0 : aura.id },

            sharedAuras: thisModel.sharedAuras.value()
                .map(function(sharedAura) {
                    return {
                        key: { id: sharedAura.key.id },

                        value: sharedAura.value
                    };
                }),

            buffs: thisModel.buffs.value()
                .map(function(buff) {
                    return {
                        key: { id: buff.key.id },

                        value: buff.value
                    };
                }),

            collectedItems: thisModel.collectedItems.value()
                .map(function (collectedItem) {
                    return { id: collectedItem.id };
                }),

            enableElixir: thisModel.elixir.active(),

            enableConsumables: thisModel.consumables.active(),

            enableGuildTalents: thisModel.guildTalents.active(),

            enableTalents: thisModel.talents.active(),

            enableAura: thisModel.aura.active(),

            enableBuffs: thisModel.buffs.active(),

            enableCollecting: thisModel.collectedItems.active(),

            lostControl: thisModel.lostControl()
        }
    };

    thisModel.getResultCode = function(character) {

        return character.code;
    };

    thisModel.setDataResult = function(character) {
        
        thisModel.displayStats.getValues()
            .foreach(function(displayModel) {

                var id = displayModel.stat().id;

                var newValue = character.stats.getValueByKeyId(id);
                var newDescriptionValue = character.statDescriptions.getValueByKeyIdOrDefault(id);

                displayModel.value(newValue);
                displayModel.descriptionValue(newDescriptionValue);
            });

        thisModel.equipments.foreach(function(equipmentModel) {

            var resultInfo = character.equipments.getValueByKeyOrDefault(function(identity) {
                return identity.slot.id === equipmentModel.identity.slot.id &&
                    identity.index === equipmentModel.identity.index;
            });

            var data = equipmentModel.data();

            if (resultInfo == null) {

                if (data != null) {
                    data.resultInfo(null);
                }
            } else {

                if (resultInfo.stampVariant != null) {
                    resultInfo.stampVariant.bonuses = context.getBonuses(resultInfo.stampVariant);
                }

                if (resultInfo.dynamicBonuses != null) {
                    resultInfo.dynamicBonuses.bonuses = context.getBonuses(resultInfo.dynamicBonuses);
                }

                data.resultInfo(resultInfo);
            }
        });
    };
    
    //-----------------------------------------------------------------------------------------------------------------

    thisModel.setData(startCharacter);
}