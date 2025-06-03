function GuildTalentBuildApplicationContext(data, facade) {

    var context = this;

    $.extend(context, new GuildTalentBuildApplicationContextBase(context, data, facade));
}