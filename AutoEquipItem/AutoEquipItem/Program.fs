open System.Security.Cryptography.X509Certificates

// hF#에 대해 http://fsharp.org에서 자세히 알아보기
// 자세한 도움말은 'F# 자습서' 프로젝트를 참조하십시오.

[<EntryPoint>]
let main _ =  
    let itemSettingPath = @"C:\Users\khh93\Dropbox\Skyrim\hunhoe2ItemSetting"
    let skyrimDataPath = @"C:\Program Files (x86)\Steam\steamapps\common\Skyrim Special Edition\Data"

    let stringToComment s = ";" + s
    let stringToCode s = "player.equipItem " + s

    let isItemSettingLine s =
        if String.exists (fun e -> e = '\t') s then 
            true
        else 
            false
    
    let changeEachCode s =
        [|for c in s -> c|]
        |> Array.skipWhile (fun e -> e <> '\t')
        |> Array.skip 1
        |> System.String
        |> stringToCode

    let textToCodes a = 
        a 
        |> Array.filter isItemSettingLine
        |> Array.map changeEachCode
        |> Array.append [|""|]
        
    let flatten a = 
        a |> Array.map (fun (x, y) -> [|x; y|]) |> Array.concat

    let texts = 
        itemSettingPath
        |> System.IO.Directory.GetFiles 
        |> Array.map System.IO.File.ReadAllLines

    let fileNames = 
        texts 
        |> Array.map (Array.item 0) 
        |> Array.map (fun e -> skyrimDataPath + @"\" + e + ".txt")

    let comments = texts |> Array.map (Array.map stringToComment)
    let codes = texts |> Array.map textToCodes
    let zippedTexts = Array.map2 Array.zip comments codes
    let flattenTexts = zippedTexts |> Array.map flatten

    let zippedFileNamesAndTexts = Array.zip fileNames flattenTexts

    let result = Array.map (fun (x, y) -> System.IO.File.WriteAllLines(x, y)) zippedFileNamesAndTexts
    
    0 // 정수 종료 코드 반환
