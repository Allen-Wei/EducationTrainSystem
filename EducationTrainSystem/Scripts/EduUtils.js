(function() {
    if (window['EduUtils']) {
        throw 'Eduutils has userd.';
    }
    window.EduUtils = {};
    EduUtils.getIndex = function(list, name, value) {
        if (!list || !list.length) { return 0;}
        var index = 0;
        for (var i = 0, iLength = list.length; i < iLength; i++) {
            if (list[i][name] == value) { index = i; break;}
        }
        return index;
    };
})();