using System.ServiceModel;

using Anon.RQ_Calc.TransferData;

namespace Rq_Calc.ServiceFacade
{
    [ServiceContract(SessionMode = SessionMode.NotAllowed)]
    public interface IFacade : ICharacterFacade, ITalentBuildFacade, IGuildTalentBuildFacade
    {

    }

    [ServiceContract]
    public interface ICharacterFacade
    {
        [OperationContract]
        CharacterStartupComplexDataRichDTO GetCharacterStartupComplexData();
        
        [OperationContract]
        CharacterResultRichDTO CalcCharacter(CharacterSourceStrictDTO characterSource);
        
        [OperationContract]
        CharacterSourceStrictDTO GetDefaultCharacter();
        
        [OperationContract]
        CharacterSourceStrictDTO DecryptCharacter(string code);
    }

    [ServiceContract]
    public interface ITalentBuildFacade
    {
        [OperationContract]
        TalentBuildStartupComplexDataRichDTO GetTalentBuildStartupComplexData();
        
        [OperationContract]
        string CalcTalentBuild(TalentBuildSourceStrictDTO talentBuild);

        [OperationContract]
        TalentBuildSourceStrictDTO GetDefaultTalentBuild();
        
        [OperationContract]
        TalentBuildSourceStrictDTO DecryptTalentBuild(string code);
    }

    [ServiceContract]
    public interface IGuildTalentBuildFacade
    {
        [OperationContract]
        GuildTalentBuildStartupComplexDataRichDTO GetGuildTalentBuildStartupComplexData();

        [OperationContract]
        string CalcGuildTalentBuild(GuildTalentBuildSourceStrictDTO talentBuild);

        [OperationContract]
        GuildTalentBuildSourceStrictDTO GetDefaultGuildTalentBuild();

        [OperationContract]
        GuildTalentBuildSourceStrictDTO DecryptGuildTalentBuild(string code);
    }
}