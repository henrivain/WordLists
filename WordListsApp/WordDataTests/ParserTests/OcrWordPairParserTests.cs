using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WordDataAccessLibrary.Generators;

namespace WordDataTests.ParserTests;
public class OcrWordPairParserTests
{
    const string TesseractOcrOutput =
        """
        Upptäck Norge!

        upptäcka, upptäcker, upptäckte, upptäckt II

        spännande (tpm)
        ett land, landet, länder, länderna 3*
        resa, reser, reste, rest II

        storslagen, storslaget, storslagna
        [stu:rsla:gen]

        folk, folket

        vänlig, vänligt, vänliga

        glad, glatt, glada

        finnas, finns, fanns, funnits IV

        se, ser, säg, sett IV

        uppleva, upplever, upplevde, upplevt II
        nägon, nägot, nägra

        het, hett, heta

        ett tips, tipset, tips, tipsen 5

        vandra, vandrar, vandrade, vandrat |
        upp

        som

        en klippa, klippan, klippor, klipporna 1
        ett hav, havet, hav, haven 5

        populär, populärt, populära

        Löydä Norja!

        löytää, huomata, havaita, keksiä

        jännittävä
        maa
        matkustaa

        suurenmoinen

        ihmiset, kansa
        ystävällinen

        iloinen

        olla olemassa

        nähdä

        kokea
        muutama, joku, jokin, eräs
        kuuma

        vinkki

        vaeltaa

        ylös

        joka, jotka, jota, joita
        kallio

        meri

        suosittu
        """;


    [Fact]
    public static void GetList_ShouldParseWords_WordPairList()
    {
        // Arrange
        var parser = new OcrWordPairParser();

        // Act
        var result = parser.GetList(TesseractOcrOutput);

        // Assert
        Assert.Equal(21, result.Count);
        
        Assert.Equal("Upptäck Norge!", result[0].NativeLanguageWord);
        Assert.Equal("Löydä Norja!", result[0].ForeignLanguageWord);
        Assert.Equal(0, result[0].IndexInVocalbulary);

        Assert.Equal("storslagen, storslaget, storslagna", result[5].NativeLanguageWord);
        Assert.Equal("suurenmoinen", result[5].ForeignLanguageWord);
        Assert.Equal(5, result[5].IndexInVocalbulary);

        Assert.Equal("populär, populärt, populära", result[20].NativeLanguageWord);
        Assert.Equal("suosittu", result[20].ForeignLanguageWord);
        Assert.Equal(20, result[20].IndexInVocalbulary);
    }

    [Fact]
    public static void ToStringList_ShouldParseMove_PairsSequentially()
    {
        // Arrange

        var parser = new OcrWordPairParser();

        // Act
        var result = parser.ToStringList(TesseractOcrOutput);

        // Assert
        Assert.Equal(42, result.Count);
        
        Assert.Equal("Upptäck Norge!", result[0]);
        Assert.Equal("Löydä Norja!", result[1]);

        Assert.Equal("storslagen, storslaget, storslagna", result[10]);
        Assert.Equal("suurenmoinen", result[11]);

        Assert.Equal("populär, populärt, populära", result[40]);
        Assert.Equal("suosittu", result[41]);
    }
}
