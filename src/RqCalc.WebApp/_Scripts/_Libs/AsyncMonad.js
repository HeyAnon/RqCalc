var TryResult = {
    
    _return: function (arg) {
        return { result: arg, isBreak: false, isSuccess: true };
    },

    error: function(error) {
        return { error: error, isBreak: false, isSuccess: false };
    },

    _break: { error: "Break", isBreak: true, isSuccess: false }
};


var Async = {

    _return: function (arg) {
        return {
            run: function (callback) {
                callback(TryResult._return(arg));
            }
        };
    },

    error: function (error) {
        return {
            run: function(callback) {
                callback(TryResult.error(error));
            }
        };
    },

    _break: {
        run: function (callback) {
            callback(TryResult._break);
        }
    },

    getIdentity: function (v) {
        return Async._return(v);
    },

    fmap: function (asyncValue, f) {

        return {
            run: function(callback) {

                asyncValue.run(function(callbackResult) {

                    if (callbackResult.result !== undefined) {
                        callback(TryResult._return(f(callbackResult.result)));
                    } else {
                        callback(callbackResult);
                    }
                });
            }
        };
    },

    bindR: function (asyncValue, f, resultSelector) {

        return {
            run: function(callback) {

                asyncValue.run(function(callbackResult) {

                    if (callbackResult.isSuccess) {

                        var nextasyncValue = f(callbackResult.result);

                        nextasyncValue.run(function(nextCallbackResult) {

                            if (nextCallbackResult.isSuccess) {
                                callback(TryResult._return(resultSelector(callbackResult.result, nextCallbackResult.result)));
                            } else {
                                callback(callbackResult);
                            }
                        });
                    } else {
                        callback(callbackResult);
                    }
                });
            }
        };
    },

    bind: function (asyncValue, f) {
        return Async.bindR(asyncValue, f, function (_, v2) {
            return v2;
        });
    },

    zip2: function (asyncValue1, asyncValue2) {

        var res1 = null;
        var res2 = null;

        return {
            run: function(callback) {

                asyncValue1.run(function(callbackResult1) {

                    res1 = callbackResult1;

                    if (res1.result === undefined) {

                        if (res2 === null || res2.result !== undefined) {
                            callback(callbackResult1);
                        }
                    } else {
                        if (res2 !== null && res2.result !== undefined) {
                            callback(TryResult._return([res1.result, res2.result]));
                        }
                    }
                });

                asyncValue2.run(function(callbackResult2) {

                    res2 = callbackResult2;

                    if (res2.result === undefined) {

                        if (res1 === null || res1.result !== undefined) {
                            callback(callbackResult2);
                        }
                    } else {
                        if (res1 !== null && res1.result !== undefined) {
                            callback(TryResult._return([res1.result, res2.result]));
                        }
                    }
                });
            }
        };
    },

    zip3: function (asyncValue1, asyncValue2, asyncValue3) {
        return Async.zipD([asyncValue1, asyncValue2, asyncValue3]);
    },

    zip4: function (asyncValue1, asyncValue2, asyncValue3, asyncValue4) {
        return Async.zipD([asyncValue1, asyncValue2, asyncValue3, asyncValue4]);
    },

    zipD: function (asyncValues) {

        return asyncValues.reduce(function (state, nextasyncValue) {

            var preNextState = Async.zip2(state, nextasyncValue);

            return Async.fmap(preNextState, function(preRes) {

                preRes[0].push(preRes[1]);

                return preRes[0];
            });
        }, Async._return([]));
    },

    ajax: function (args) {

        return {
            run: function(callback) {

                $.ajax({
                    type: args.type,
                    url: args.url,
                    data: args.data,
                    contentType: args.contentType,
                    dataType: args.dataType,
                    success: function(data) {
                        callback(TryResult._return(data));
                    },
                    error: function(xhr, status, error) {
                        callback(TryResult.error(xhr.responseText));
                    }
                });
            }
        };
    },

    get : function(url) {

        return {
            run: function(callback) {

                $.get(url, function(data) {
                    callback(TryResult._return(data));
                }).fail(function(error) {
                    callback(TryResult.error(error));
                });
            }
        };
    },

    fromFunc : function(func) {
        
        return {
            run : function(callback) {

                var innerFunc = func();

                return innerFunc.run(callback);
            }
        }
    },

    toLastCall: function (asyncValue) {

        var lastState;

        return {
            run: function(callback) {

                var currentState = lastState = {};

                asyncValue.run(function(tryResult) {

                    if (lastState === currentState) {
                        callback(tryResult);
                    } else {

                        callback(TryResult._break);
                    }
                });
            }
        };
    },

    withCatch: function (asyncValue) {

        return {
            run: function(callback) {

                asyncValue.run(function(tryResult) {

                    if (tryResult.result === undefined) {
                        callback(TryResult._return(null));
                    } else {
                        callback(tryResult);
                    }
                });
            }
        };
    },

    throttle: function (asyncValue, delay, breakSuccess) {

        var lastTimerId = undefined;
        var lastCallback = undefined;

        return {
            run: function (callback) {

                if (lastTimerId != undefined) {
                    clearTimeout(lastTimerId);
                    lastCallback(TryResult._break);
                }

                lastTimerId = setTimeout(function () {
                    
                    if (callback !== lastCallback) {
                        return;
                    }
                    
                    asyncValue.run(function (tryResult) {

                        var isLastCallback = callback === lastCallback;

                        if (isLastCallback) {
                            lastTimerId = undefined;
                            lastCallback = undefined;
                        }

                        if (tryResult.isSuccess) {

                            if (!breakSuccess || isLastCallback) {
                                callback(tryResult);
                            } else {
                                callback(TryResult._break);
                            }

                        } else {
                            callback(tryResult);
                        }

                    });

                }, delay);

                lastCallback = callback;
            }
        };
    }
};