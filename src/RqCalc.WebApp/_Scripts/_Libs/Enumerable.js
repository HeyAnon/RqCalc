Array.prototype.emptyOrContains = function (value) {

    return !this.any() || this.contains(value);
};

Array.prototype.contains = function (value) {

    var res = $.inArray(value, this);

    return res !== -1;
};

Array.prototype.distinct = function () {

    return this.groupBy(function(v) {
        return v;
    }).map(function(g) {
        return g.key;
    });
};

Array.prototype.groupBy = function (keySelector) {

    var res = [];

    this.foreach(function(element) {
        var key = keySelector(element);

        var arr = res.getValueByKeyOrDefault(key);

        if (arr == null) {
            res.push({ key: key, value: [element] });
        } else {
            arr.push(element);
        }
    });

    return res;
};

Array.prototype.selectMany = function (resultSelector) {

    if (resultSelector == undefined) {
        resultSelector = function(v) {
            return v;
        };
    }

    var res = this.map(resultSelector).reduce($.merge, []);

    return res;
};

Array.prototype.foreach = function (action) {

    $.each(this, function(_, v) {
        action(v);
    });
};

Array.prototype.except = function (otherElements) {

    return this.filter(function (element) {
        return !otherElements.contains(element);
    });
};

Array.prototype.any = function (condition) {
    
    for (var i = 0; i < this.length; i++) {
    
        if (condition == undefined || condition(this[i])) {
            return true;
        }
    }

    return false;
};

Array.prototype.all = function (condition) {

    for (var i = 0; i < this.length; i++) {

        if (!condition(this[i])) {
            return false;
        }
    }

    return true;
};

Array.prototype.orderBy = function (keySelector) {

    var arr = this.slice(0);

    arr.sort(function(v1, v2) {

        var key1 = keySelector(v1);
        var key2 = keySelector(v2);

        return key1 > key2 ? 1 : key1 < key2 ? -1 : 0;
    });

    return arr;
};

Array.prototype.reverseC = function () {

    var arr = this.slice(0);

    arr.reverse();

    return arr;
};

Array.prototype.take = function (count) {

    if (count < 1) {
        return [];
    } else {
        return this.slice(0, Math.min(count, this.length));
    }
};

Array.prototype.skip = function (count) {

    if (count < 1) {
        return this.slice(0);
    } else if (count >= this.length) {
        return [];
    } else {
        return this.slice(count, this.length);
    }
};

Array.prototype.takeWhile = function (filter) {

    if (this.length === 0) {
        return [];
    }

    var index = 0;

    while (index < this.length && filter(this[index])) {
        index++;
    }

    return this.take(index);
};


Array.prototype.skipWhile = function (filter) {

    if (this.length === 0) {
        return [];
    }

    var index = 0;

    while (index < this.length && filter(this[index])) {
        index++;
    }

    return this.skip(index);
};

Array.prototype.single = function(filter) {

    var res = filter == undefined ? this : this.filter(filter);

    if (res.length === 0) {
        throw new Error("Item not found");
    } else if (res.length === 1) {
        return res[0];
    } else {
        throw new Error("More one item");
    }
};

Array.prototype.singleOrDefault = function (filter) {

    var res = filter === undefined ? this : this.filter(filter);

    if (res.length === 0) {
        return null;
    } else if (res.length === 1) {
        return res[0];
    } else {
        throw new Error("More one item");
    }
};

Array.prototype.first = function(filter) {

    var res = filter === undefined ? this : this.filter(filter);

    if (res.length === 0) {
        throw new Error("Item not found");
    } else {
        return res[0];
    }
};

Array.prototype.last = function (filter) {

    var res = filter === undefined ? this : this.filter(filter);

    if (res.length === 0) {
        throw new Error("Item not found");
    } else {
        return res[res.length - 1];
    }
};

Array.prototype.firstOrDefault = function (filter) {

    var res = filter === undefined ? this : this.filter(filter);

    if (res.length === 0) {
        return null;
    } else {
        return res[0];
    }
};

Array.prototype.lastOrDefault = function (filter) {

    var res = filter === undefined ? this : this.filter(filter);

    if (res.length === 0) {
        return null;
    } else {
        return res[res.length - 1];
    }
};

Array.prototype.isIntersected = function (other) {

    return this.any(function (v) {
        return other.contains(v);
    });
};

Array.prototype.sum = function (getValue) {

    var res = this.reduce(function (state, v) {

        return state + (getValue === undefined ? v : getValue(v));
    }, 0);

    return res;
};

Array.prototype.getById = function (id) {
    
    return this.single(function (item) {
        return item.id === id;
    });
};

Array.prototype.getByIdOrDefault = function (id) {

    return this.singleOrDefault(function (item) {
        return item.id === id;
    });
};

Array.prototype.getValueByKey = function (filter) {

    var pair = $.isFunction(filter) ? this.single(function (pair) {
        return filter(pair.key);
    }) : this.single(function (pair) {
        return pair.key === filter;
    });

    return pair.value;
};

Array.prototype.getValueByKeyOrDefault = function (filter) {

    var pair = $.isFunction(filter) ? this.singleOrDefault(function(pair) {
        return filter(pair.key);
    }) : this.singleOrDefault(function(pair) {
        return pair.key === filter;
    });

    return ObjHelper.maybe(pair, function(p) {
        return p.value;
    });
};


Array.prototype.getValueByKeyId = function (key) {

    return this.single(function (pair) {
        return pair.key.id === key;
    }).value;
};

Array.prototype.getValueByKeyIdOrDefault = function (key) {

    var pair = this.singleOrDefault(function(pair) {
        return pair.key.id === key;
    });

    return ObjHelper.maybe(pair, function(p) {
        return p.value;
    });
};



Array.prototype.getKeys = function () {

    return this.map(function (pair) {
        return pair.key;
    });
};

Array.prototype.getValues = function () {
    
    return this.map(function (pair) {
        return pair.value;
    });
};

Array.prototype.containsKey = function (key) {

    return this.getKeys().contains(key);
};

Array.prototype.toDict = function(keySelector, valueSelector) {
    return this.map(function(value) {
        return { key: keySelector(value), value: valueSelector == undefined ? value : valueSelector(value) };
    });
};

Array.prototype.changeKey = function (keySelector) {
    return this.map(function (pair) {
        return { key: keySelector(pair.key), value: pair.value };
    });
};

var Enumerable = {
    range: function(start, count) {

        var arr = new Array(count);

        for (var i = 0; i < count; i++) {
            arr[i] = start + i;
        }

        return arr;
    }
};