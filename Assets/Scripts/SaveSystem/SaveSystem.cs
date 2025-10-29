using System.IO;
using UnityEngine;

public static class SaveSystem
{
    private static string SavePath => Path.Combine(Application.persistentDataPath, "save.json");

    public static void Save(GameStateData data)
    {
        try
        {
            string json = JsonUtility.ToJson(data, true);
            File.WriteAllText(SavePath, json);
            Debug.Log($"[SaveSystem] Jogo salvo em: {SavePath}");
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"[SaveSystem] Erro ao salvar: {ex}");
        }
    }

    public static GameStateData Load()
    {
        if (!File.Exists(SavePath))
        {
            throw new GameNotFoundException();
        }

        try
        {
            string json = File.ReadAllText(SavePath);
            var data = JsonUtility.FromJson<GameStateData>(json);
            Debug.Log("[SaveSystem] Jogo carregado com sucesso.");
            return data;
        }
        catch (System.Exception ex)
        {
            throw new BadFormatGameException(ex.Message);
        }
    }

    public static bool HasSave() => File.Exists(SavePath);

    public static void DeleteSave()
    {
        if (File.Exists(SavePath))
            File.Delete(SavePath);
    }
}
