function BuffsModel(buffs, startupBuffs) {

    var thisModel = this;

    thisModel.items = buffs.map(function (buff) {

        return {

            buff: buff,

            stacks: Enumerable.range(1, buff.maxStackCount).map(function (index) {

                return {

                    text: buff.bonuses.map(function (buffBonus) {

                        return TextTemplate.evaluate(buffBonus.template, [buffBonus.perStackValue * index]);
                    }).join("\r\n"),

                    index: index
                }
            }),

            value: ko.observable(startupBuffs.getValueByKeyOrDefault(buff))
        };
    });

    thisModel.clear = function() {
        
        thisModel.items.foreach(function (pair) {

            pair.value(null);
        });
    };
}


function BuffsWindowModel(context, character) {
    
    var thisModel = this;

    var startupBuffs = character.buffs.value();
    
    {
        var $class = character.class();
        var currentLevel = character.level();
        var currentTalents = character.talents.value();

        var classBuffs = $class.buffs.filter(function(buff) {
            return MainHelper.Buff.isAllowed(buff, currentLevel, currentTalents);
        });

        thisModel.classBuffs = new BuffsModel(classBuffs, startupBuffs);
    }
    
    {
        thisModel.sharedPositiveBuffs = new BuffsModel(context.sharedBuffs.filter(function (sharedBuff) {
            return !sharedBuff.isNegate;
        }), startupBuffs);
    }

    {
        thisModel.cardBuffs = new BuffsModel(MainHelper.CharacterSource.getCardBuffs(character), startupBuffs);
    }


    {
        thisModel.stampBuffs = new BuffsModel(MainHelper.CharacterSource.getStampBuffs(character), startupBuffs);
    }

    {
        thisModel.sharedNegativeBuffs = new BuffsModel(context.sharedBuffs.filter(function (sharedBuff) {
            return sharedBuff.isNegate;
        }), startupBuffs);
    }

    var models = [thisModel.classBuffs, thisModel.sharedPositiveBuffs, thisModel.cardBuffs, thisModel.stampBuffs, thisModel.sharedNegativeBuffs];
    
    thisModel.clear = function () {

        models.foreach(function(model) {
            model.clear();
        });
    };

    thisModel.getResult = function () {
        
        return models.selectMany(function (model) {
            return model.items;
        }).map(function (pair) {
            return {
                key: pair.buff,
                value: pair.value()
            }
        }).filter(function (pair) {
            return pair.value != null;
        });
    }
}