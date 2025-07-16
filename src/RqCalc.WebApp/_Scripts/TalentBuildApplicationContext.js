function TalentBuildApplicationContext(data, facade) {

    var context = this;

    $.extend(context, new TalentBuildApplicationContextBase(context, data, facade));
}