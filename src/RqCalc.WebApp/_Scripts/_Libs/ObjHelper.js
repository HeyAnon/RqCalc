String.prototype.minMax = function(min, max) {
    return Math.min(max, Math.max(min, this));
};

String.prototype.toStartLowerCase = function() {
    return this.length === 0 ? this : this[0].toLowerCase() + this.substring(1);
};

String.prototype.toStartUpperCase = function() {
    return this.length === 0 ? this : this[0].toUpperCase() + this.substring(1);
};


Number.prototype.round = function(places) {
    return +(Math.round(this + "e+" + places) + "e-" + places);
};

Number.prototype.minMax = function(min, max) {

    return Math.min(max, Math.max(min, this));
};



var FuncHelper = {
    identity: function(v) {

        return v;
    }
}


var ObjHelper = {
   
    clone : function(obj) {

        var res = JSON.parse(JSON.stringify(obj));

        return res;
    },
    
    getAllParents : function(obj) {

        var arr = [];

        for (var state = obj; state != null; state = state.parent) {
            arr.push(state);
        }

        return arr;
    },

    getAllChildren: function (obj) {
        
        return $.merge([obj], obj.children.selectMany(ObjHelper.getAllChildren));
    },

    toLowerData: function (source) {

        if (source === null) {
            return null;
        }
        else if (typeof (source) === "object") {

            if (Array.isArray(source)) {

                return source.map(ObjHelper.toLowerData);

            } else {
                var newObj = {};

                for (var p in source) {
                    newObj[p.toStartLowerCase()] = ObjHelper.toLowerData(source[p]);
                }

                return newObj;
            }
        } else {
            return source;
        }
    },

    toUpperData : function(source) {

        if (source === null) {
            return null;
        }
        else if (typeof (source) === "object") {

            if (Array.isArray(source)) {

                return source.map(ObjHelper.toUpperData);

            } else {
                var newObj = {};

                for (var p in source) {
                    newObj[p.toStartUpperCase()] = ObjHelper.toUpperData(source[p]);
                }

                return newObj;
            }
        } else {
            return source;
        }
    },

    isNumber: function(value) {
        return typeof value === "number";
    },


    maybe: function(value, selector) {
        return value === null ? null : selector(value);
    }
};

var TextTemplate = {

    evaluate: function (text, parameters) {

        for (var i = 0; i < parameters.length; i++) {

            text = text.replace("{" + i + "}", parameters[i]);
        }

        return text;
    }
};

function guid() {
    function s4() {
        return Math.floor((1 + Math.random()) * 0x10000)
            .toString(16)
            .substring(1);
    }
    return s4() + s4() + '-' + s4() + '-' + s4() + '-' +
        s4() + '-' + s4() + s4() + s4();
}