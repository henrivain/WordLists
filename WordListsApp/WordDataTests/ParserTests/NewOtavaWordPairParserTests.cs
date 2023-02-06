using NuGet.Frameworks;
using WordDataAccessLibrary.Generators;

namespace WordDataTests.ParserTests;
public class NewOtavaWordPairParserTests
{
    readonly IWordPairParser _parser = new NewOtavaWordPairParser();

    [Fact]
    public void ToStringList_ShouldRemove_PlayWords_IfInEndOfLine()
    {
        // Arrange 
        var input =
            """
            näyttää jltak Play

            se, ser, såg, sett IV ut

            Hän näyttää hyvältä. Play

            Hon ser bra ut.

            ulkonäkö Play

            utseende, -t
            """;

        var expectedOut =
            """
            näyttää jltak
            se, ser, såg, sett IV ut
            Hän näyttää hyvältä.
            Hon ser bra ut.
            ulkonäkö
            utseende, -t
            """;

        // Act
        var output = _parser.ToStringList(input);

        // Assert
        Assert.Equal(expectedOut, string.Join(Environment.NewLine, output));
    }

    [Fact]
    public void ToStringList_ShouldNotRemove_PlayWords_InTheMiddle()
    {
        // Arrange
        var input =
            """
            Play a game
            
            Pelaa peli
            """;

        var expectedOut =
            """
            Play a game
            Pelaa peli
            """;

        // Act 
        var result = _parser.ToStringList(input);

        // Assert
        Assert.Equal(expectedOut, string.Join(Environment.NewLine, result));
    }

    [Fact]
    public void ToStringList_ShouldRemove_AnythingBetweenBrackets()
    {
        // Arrange  
        // Also removes the empty line with only "/" -symbol
        var input =
            """
            ha IV runda kinder
            [tʃindär] / [çinder]
            olla parta/viikset
            """;

        var expectedOut =
            """
            ha IV runda kinder
            olla parta/viikset
            """;

        // Act
        var result = _parser.ToStringList(input);

        // Assert
        Assert.Equal(expectedOut, string.Join(Environment.NewLine, result));
    }

    [Fact]
    public void ToStringList_ShouldSee_NextLineFrom_LineEndingInForwardslash_AsSameLine()
    {
        // Arrange
        var input =
            """
            vara IV ljushårig, -t, -a /
            mörkhårig, -t, -a   
            olla kalju
            """;

        var expectedOut =
            """
            vara IV ljushårig, -t, -a / mörkhårig, -t, -a
            olla kalju
            """;

        // Act
        var result = _parser.ToStringList(input);

        // Assert
        Assert.Equal(expectedOut, string.Join(Environment.NewLine, result));
    }

    [Fact]
    public void ToStringList_ShouldRemove_UnnecessarySlashes()
    {
        // Arrange
        var input =
            """
            olla laiha/hoikka/lihava/urheilullinen Play
            vara IV mager, magert,
            magra / slank, -t, -a /
            tjock, -t, -a [tʃok] / [çok] /
            sportig, -t, -a
            """;

        var expectedOut =
            """
            olla laiha/hoikka/lihava/urheilullinen
            vara IV mager, magert, magra / slank, -t, -a / tjock, -t, -a  / sportig, -t, -a
            """;

        // Act
        var result = _parser.ToStringList(input);

        // Assert
        Assert.Equal(expectedOut, string.Join(Environment.NewLine, result));
    }


    [Fact]
    public void ToStringList_ShouldHandle_LongList()
    {
        string input =
            """
            näyttää jltak Play

            se, ser, såg, sett IV ut
            Hän näyttää hyvältä. Play

            Hon ser bra ut.
            ulkonäkö Play

            utseende, -t [ʉːtseːende]

            olla pitkät/lyhyet/kiharat hiukset Play

            ha IV långt / kort [kort] /
            lockigt hår

            olla vaaleat/tummat/punaiset hiukset Play

            ha IV ljust [jʉːst] / mörkt / rött hår

            olla vaaleahiuksinen/tummahiuksinen Play

            vara IV ljushårig, -t, -a [jʉːs-] /
            mörkhårig, -t, -a
            olla kalju Play

            vara IV skallig, -t, -a
            olla kapeat/pyöreät kasvot Play

            ha IV smalt/runt ansikte
            olla pieni/suuri suu Play

            ha IV liten/stor mun

            olla pieni/suuri nenä Play

            ha IV liten/stor näsa
            olla pyöreät posket Play

            ha IV runda kinder
            [tʃindär] / [çinder]
            olla parta/viikset Play

            ha IV skägg [ʃeg] /
            mustasch [mʉstaːʃ]
            olla silmälasit Play

            ha IV glasögon
            olla pitkä/lyhyt Play

            vara IV lång, -t, -a /
            kort, -t, -a [kort]
            olla laiha/hoikka/lihava/urheilullinen Play

            vara IV mager, magert,
            magra / slank, -t, -a /
            tjock, -t, -a [tʃok] / [çok] /
            sportig, -t, -a
            """;

        string expectedOut =
            """
            näyttää jltak
            se, ser, såg, sett IV ut
            Hän näyttää hyvältä.
            Hon ser bra ut.
            ulkonäkö
            utseende, -t
            olla pitkät/lyhyet/kiharat hiukset
            ha IV långt / kort  / lockigt hår
            olla vaaleat/tummat/punaiset hiukset
            ha IV ljust  / mörkt / rött hår
            olla vaaleahiuksinen/tummahiuksinen
            vara IV ljushårig, -t, -a  / mörkhårig, -t, -a
            olla kalju
            vara IV skallig, -t, -a
            olla kapeat/pyöreät kasvot
            ha IV smalt/runt ansikte
            olla pieni/suuri suu
            ha IV liten/stor mun
            olla pieni/suuri nenä
            ha IV liten/stor näsa
            olla pyöreät posket
            ha IV runda kinder
            olla parta/viikset
            ha IV skägg  / mustasch
            olla silmälasit
            ha IV glasögon
            olla pitkä/lyhyt
            vara IV lång, -t, -a / kort, -t, -a
            olla laiha/hoikka/lihava/urheilullinen
            vara IV mager, magert, magra / slank, -t, -a / tjock, -t, -a  / sportig, -t, -a
            """;

        // Act
        List<string> result = _parser.ToStringList(input);

        // Assert
        Assert.Equal(expectedOut, string.Join(Environment.NewLine, result));

    }
}
