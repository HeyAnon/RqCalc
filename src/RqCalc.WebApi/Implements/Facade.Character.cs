using System;

using Framework.Core;
using Framework.Core.Serialization;

using Anon.RQ_Calc.TransferData;

namespace Rq_Calc.ServiceFacade
{
    public partial class Facade
    {
        public CharacterStartupComplexDataRichDTO GetCharacterStartupComplexData()
        {
            return this._characterStartupComplexData;
        }

        public CharacterSourceStrictDTO GetDefaultCharacter()
        {
            return this._defaultCharacter;
        }

        public CharacterResultRichDTO CalcCharacter(CharacterSourceStrictDTO characterSource)
        {
            if (characterSource == null) throw new ArgumentNullException(nameof(characterSource));

            return this.Evaluate((context, mappingService) =>
            {
                return characterSource.ToDomainObject(mappingService)
                                      .Pipe(domainObj => context.Calc(domainObj))
                                      .Pipe(result => new CharacterResultRichDTO(result, mappingService));
            });
        }

        public CharacterSourceStrictDTO DecryptCharacter(string code)
        {
            if (string.IsNullOrWhiteSpace(code)) throw new ArgumentException("Value cannot be null or whitespace.", nameof(code));

            return this.Evaluate((context, mappingService) =>
            {
                return context.Serializer.Deserialize(code)
                                         .Pipe(characterSource => new CharacterSourceStrictDTO(characterSource));
            });
        }
    }
}