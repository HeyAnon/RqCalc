function SingleCardGroup(card) {

    var thisModel = this;

    thisModel.activeCard = ko.observable(card);
    thisModel.name = card.name;
    thisModel.isLegacy = card.isLegacy;

    thisModel.cards = ko.observable(card);

    thisModel.getAvailableCards = function() {
        return [card];
    }

    thisModel.refreshSource = function() {
        
    }

    thisModel.visible = false;
}

function MultiCardGroup(name, baseCards) {

    var thisModel = this;

    thisModel.activeCard = ko.observable(null);
    thisModel.name = name;

    thisModel.isLegacy = baseCards.all(function(c) {
        return c.isLegacy;
    });

    thisModel.showLegacy = ko.observable(false);
    thisModel.showLegacy.subscribe(function (_) {
        thisModel.refreshSource();
    });

    thisModel.cards = ko.observable();

    thisModel.getAvailableCards = function () {
        return baseCards;
    }

    thisModel.refreshSource = function () {

        var newCards = baseCards.filter(function(card) {
            return !card.isLegacy || thisModel.showLegacy();
        });

        thisModel.cards(newCards);

        if (thisModel.activeCard() == null) {
            thisModel.activeCard(newCards.lastOrDefault());
        }
    }

    thisModel.visible = true;

    thisModel.refreshSource();
}

function CardsWindowModel(context, evaluateStats, equipmentType, cardIndex, $class, startupCard, equipmentClass) {

    var thisModel = this;

    $.extend(thisModel, new SwitchItemDialogModel([], startupCard));


    thisModel.showLegacy = ko.observable(startupCard != null && startupCard.isLegacy);
    thisModel.showLegacy.subscribe(function (_) {
        refresh();
    });


    thisModel.clear = function () {
        thisModel.pureModel.selectedItem(null);
    };

    thisModel.getResult = function () {

        var activeGroup = thisModel.pureModel.selectedItem();

        return activeGroup == null ? null : activeGroup.activeCard();
    };

    thisModel.getDescription = function (card) {

        return new CardDescriptionModel(context, evaluateStats, card);
    };

    var baseGroups =

        context.cards.filter(function (card) {
            return MainHelper.Card.isAllowedEE(card, equipmentType, equipmentClass) && (card.type.element == null || cardIndex === 0);
        }).orderBy(function (card) {
            return -MainHelper.Bonus.getOrderIndex(card, $class);
        }).orderBy(function (card) {
            return card.type.id;
        }).groupBy(function (card) {
            return card.group == null ? guid() : card.group.key;
        }).map(function (cardGroup) {
            return cardGroup.value.length == 1
                ? new SingleCardGroup(cardGroup.value.single())
                : new MultiCardGroup(cardGroup.key, cardGroup.value.orderBy(function (card) {
                    return card.group.orderKey;
                }));
            });

    thisModel.hasLegacy = baseGroups.any(function(g) {
        return g.isLegacy;
    });

    {
        var startGroup = startupCard == null ? null : baseGroups.singleOrDefault(function (cardGroup) {
            return cardGroup.getAvailableCards().contains(startupCard);
        });

        thisModel.pureModel.selectedItem(startGroup);

        if (startGroup != null) {
            startGroup.activeCard(startupCard);
        }
    }

    refresh();

    //---------------------------------------------------------------------
    function refresh() {

        var newCardGroups = baseGroups.filter(function(cardGroup) {
            return !cardGroup.isLegacy || thisModel.showLegacy();
        });


        newCardGroups.foreach(function (cardGroup) {

            if (cardGroup.showLegacy != undefined) {
                cardGroup.showLegacy(thisModel.showLegacy());    
            }
        });
        
        thisModel.pureModel.items(newCardGroups);
        

        if (thisModel.pureModel.selectedItem() == null) {

            thisModel.pureModel.selectedItem(newCardGroups.firstOrDefault());
        }
    }
}