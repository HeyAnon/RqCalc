using Framework.Core.Serialization;

using Microsoft.Extensions.DependencyInjection;

using RqCalc.Application;
using RqCalc.Model;

namespace RqCalc.Tests;

public class MainTest(IServiceProvider serviceProvider)
{
    [Theory]
    [MemberData(nameof(TestDeserializeCodes_AllCodesDeserialized_TestData))]
    public void TestDeserializeCodes_AllCodesDeserialized(string code)
    {
        // Arrange
        var characterStringSerializer = serviceProvider.GetRequiredService<ISerializer<string, ICharacterSource>>();

        var calc = serviceProvider.GetRequiredService<ICharacterCalculator>();

        // Act
        var act = () =>
                  {
                      var character = characterStringSerializer.Parse(code);

                      var calcResult = calc.Calculate(character);
                  };

        // Assert
        act.Should().NotThrow<Exception>();
    }

    public static IEnumerable<object?[]> TestDeserializeCodes_AllCodesDeserialized_TestData()
    {
        yield return ["EBrBR5zlJAiZ/8tFtE572at28t0NbmR7PLp3Mv32cb3cwzRNF7s7lfb80g1PuuHJdnqqkqw9HAYB5n1HEiEB"];
        yield return ["FAeCX5zlpAQh85PJgdNoR3vPzs6zjY1qbxvFTl7dPp5pT7tjc6fElle054n2POdOg9U3u8Fpmo75wOdSFgBBBeYgBSWttwAA"];
        yield return ["DgfBL85yUoKQ+ckncmC0yb23s/tsY6Pa22Cxk2W1j2fY3O/Y3FfY8Mo3PvnG59x5qxqze/t5HhQOfgAA"];
        yield return ["DgfJL85yUoKQqcnkU6AHuYFgmyfWaWC61chmq8EEI9GA0CDAaTjDNhTUjYK6McJ3Vu1VSEgCgCkOvwAA"];
        yield return ["FBSO38VZTkoQMj+5pyim9rKX7Ows292Ydrow7GRa7eOq2N6psO13bnjODU+z01P1fWz3sGnCKX+2XAJAoAAA"];
        yield return ["EBXE7+IsJyUImT9MVgVOWx5JdvaN3exMO72IdvKp9rExtncp7Dodm79j89dsf1bdURtcEAQ9g1sAAA=="];
        yield return ["FAeCX5zlpAQh85MDJ/JoR3vhzs6zjY1+bxvFTl7dPp5pT7tjc6fElle054n2POdmg1Uyu8Fpmo75wOdSFgDhAgA="];
        yield return ["FBqCjzjLSRAyP3gcYJ32spfu5Lwb3MjscePeyfTbx/VyDzdNs9jVM7DnV+14qh1Ps9Nglczaw2IQxLzvSALgPw=="];
        yield return ["DhjBH86SIGT68eA0Hu1o7+3sbNvYmDa4Mezkte3jmfa0OzZ3Jra88n1Pvu+JNhus0uwGp2kacYFrAAA="];
        yield return ["DhjBH86SIGT6cUAlHu1o7+3kbNvYmDa48e7ktezjmfa0OzZ3Jra88o1PvvGJNhus0uwGp2kacYFrAAA="];
        yield return ["EBjHH86SIGT+8XhwGu1oL+3kbNvYmDa4Mexkuu3jmva0Ozb3TGz5HfueY98TbTZYpdkNTtM04gLXyo2GBA=="];
        yield return ["FBSO38VZTkoQMv8YGGCm9rKX7Ows293od7ow7GQa7uOq2N6zsO13bnjODc+702D1fWz3sGnCKX+yXAJAuAAA"];
        yield return ["FBSO38VZTkoQMv+4Gfil9rKX7Ows293od7oR72Qa7uOq2N6zsO137nfO/U6z02D1fWz3sGnCKX+yXAJAuAAA"];
        yield return ["ERTE7+IsJyUImV88DqhSO9pIdnaW7W5MO10YNvgIt7VRbO9Z2HaK9nzRnq/cbLDaPrY7qCjglD9ZLgEA"];
        yield return ["DhHBG4uznJQgZH7yiRzI7fXvbmzd2/gM7nFw3ckyuY9nsLVfsbtzodM17bSnnXaz1V3N4eMun6ap7HOCIKS6+AUA"];
        yield return ["FBGCNxZnOSlByPzkEzmQ2+vetbF3b2MjuMfBdSfL5D6eit2dFjpd00572mm3W93VZPizyQVBUPY5QRCkungFAIKBBg=="];
        yield return ["EBHBG4uznJQgZH7yiRzI7XXvbuzd29gI7nFw3ckyuY9nsLnj2MlpodM17bSnnXaz1V3N4eMun6ap7HOCIKS6eAUA"];
        yield return ["FBGCNxZnOSlByPzkMSpQ3etesrN3b2Ojs8fFdSeL5D6uil2dght+x67/2PWnm/7VZP2zyQVBUPY5QRCkungFhGz/AQ=="];
        yield return ["FBGCNxZnOSlByPzkMSpQ3etesrN3b2Ojs8fFdSeL5D6uweaOxa5OwQ2/Y9d/7PrTTf9qsm7e5c/zKPucIAhSXbwCQrb/"];
        yield return ["CRHBm8VZTkoQMj85gBqXdrT3dnLuDW4E97gx7mQS3Mcz2NOu2N1Xd1yiPU+05yk3G6yZbd7lIE2XfQ42zdW51AA="];
        yield return ["DhXB7+IsJyUImX9MQIVPO9qLdnKO7W5MO90YdjKq9vFUbO8s7XqNW55xy9NsNljVPRv8axr/hzQAAA=="];
        yield return ["DhXB7+IsJyUImZ8cUIVPO9qLdjKP7e5MOz0adjKq9jFVbu8s77pFe+5oz11u9lSlPRscpGn/hzQAAA=="];
        yield return ["FBWC38VZTkoQMr8YHGiddrSX7OQc292YdroR72Ta7eOq2NWpsOt37nfO/c672WCV7NngYdPwf0hDuRCy/Qc="];
        yield return ["AgrA16QxPTlAPp1whFdhDb746czwINhhbfnhr3lpR2kaOAAUDKkJTzrhVku8452sXArStDz/0Q=="];
        yield return ["BRHBm8VZThpzk0/kQK5SeNc2wK4jZ7COg2tOlslKOUVbAqUmYLppTDWNJisA/OBGQRBMefAL"];
        yield return ["AxXF7+IsJyUImZ8MXgVOWx5Jdnau7W5MO12IdvKp9rFRbPMrm27H5ufY/DS7n1V31AYXBEHP4RY="];
        yield return ["AxXB7+IsJyUImZ9MPgVOWx5Jdnau7W5MO12IdvKp9rFRbPMru27H5ufY/DTbn1V31AYXBEHP4RY="];
        yield return ["EArGV5zlpAQh05EDJ/JQcwPDbp64p712VyPhrhbdjkYJG1oEPT3O2y2Fdq/Q7oU7nlV7zy4OggBz3tIAAA=="];
        yield return ["BRfDL85yUoKQ6ckB8mm0yZu3s+1sY6Pa21mxs1ezr4ljc1PX8kg3XOmG69l5qxoze3tZFs8H4AA="];
        yield return ["DxfHL85yUoKQ+cdiVeC3yaO0n+3saqfa21ixs8O9p8nb3OjY1RXccDr2W8d+69n2o7qye/t5HhQOfgAa"];
        //yield return ["CRvD68VZTkoQMj85gJuedvRU7eSJG5zK7PHs29rk29RMsbun7LlUW79q69ds9lTVkU0uCIKDBAA="];
        //yield return ["CRvD68VZTkoQMj85gJuednRT7eSJG5zK7PHs29rk29RMsbun67mkG750w9ds9lTVkU0eLouDBAA="];
        //yield return ["CRvD68VZTkoQMj85gJuedvRU7eSJG5zK7PHs29rk29RMsbun7LmUW75yy9ds9lTVkU3+JYeDBAA="];
        yield return ["FBqGrzjLSQlCpicHTuTTjm7SnZx3g1PFPW50W/t++5td7vJlWdTdPRt6LumGL93wnZsNVp+sXR5aFk3jPwAA/wE="];
        yield return ["EBrDV5zlpAQh05MDJ/JpR0/VTp67walsj0fd1ibf/maXezhJkmJ3z++eS7nlK7d8zWZP1Ze1h7/j0DT+AwAA"];
        yield return ["ERrDV5zlpAQh05MDAfJpRxfVTs67wY3MHje6rX2//c0ud7miKIrdPQp7LtOeb9rzlZsNVpusXQ4GAU3jPwAA"];
        yield return ["DAfHL85yUoKQ+cnkU2C0yb20s/tsY6Pa22Kxk021j+vb3O/Y1Vc2/PKNT77xObfdqsbs3n6eB4XDPyA3"];
        yield return ["EheOX5zlpAQh85PFqcBok3vJzu6zjY1qb4PFTpbVPp5vc79jV6fghtex8T423uu2W9U5u7ef54HCwQ8QsgE="];
        yield return ["EBfHL85yUoKQ+cVfQWu0o5G0k/Psb6Pa2Vixs9e9p8nb0+jY1SSw5ZHut9L91rPZYPVld/PzPKiDgQcA"];
        yield return ["EheOX5zlpAQh84fDgdZoL3vJzs6zjY1+bxvFTqbNPq5pT7tjc6fElt+x7zn2PfFOg9U3u8Fpmo75wOdSFiEB"];
        yield return ["DBXH7+IsJyUImV88RoxOexlJdnaO7W5MO12IdzLt9nFV7Oorm37H5ufY/DQ7nVV11AYXBEHP4RbDXEgD"];
        yield return ["FAKCT3GWkxKEzE8OnMjfHe09OzmTDW5Ut7Qx7uQb28dUsbtnec9l2vFMO554s8Fq/LPJm6YJ/g99AOA/"];
        yield return ["DgvGx+IsJyUImYocOJGHmhsYdvPEPQ1cdDUy7mpw6UgUQCIAAIKehtN2Q6HdKLQb4Y5n1VaCRAAAYAAAAA=="];
        yield return ["FNGCd7MfznIhZKjsMPUnn8iB3Fb2nm28eysbyY08rps5JDeyDDZyLPbxSW805hudfKOzbjRXdVc3sCzLnYBCMXWZQOPlhBwe"];
        yield return ["FBuOj8VZTkoQMj+ZHDiddrTX7OSMG9zI7HFj38n328c12NNN0yx2dyrv+VU7nmrH8242WCVLe1oMgk4CSALgPw=="];
        yield return ["FBuOj8VZTkoQMv84HFiddrTX7OSMG9zI7HFj38n328dVsbtTec+v2vFUO553s8EqGdnOYtN0EkASAP8B"];
        yield return ["FBiADQDIAWWgpMm+Kjtv1tXJpcHAFifKJB/Parle1KJncWF1YFwPjO12893nebfKsgQAAAAA"];
    }
}
