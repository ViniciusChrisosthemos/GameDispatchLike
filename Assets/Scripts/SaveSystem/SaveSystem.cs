using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public static class SaveSystem
{
    private static string SaveFolder => Application.persistentDataPath;

    public static void Save(GameStateData data)
    {
        try
        {
            string saveFilePath = Path.Combine(SaveFolder, data.SaveFile);
            string json = JsonUtility.ToJson(data, true);

            File.WriteAllText(saveFilePath, json);

            Debug.Log($"[SaveSystem] Jogo salvo em: {saveFilePath}");
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"[SaveSystem] Erro ao salvar: {ex}");
        }
    }

    public static GameStateData Load(string saveFile)
    {
        string saveFilePath = Path.Combine(SaveFolder, saveFile);

        if (!File.Exists(saveFilePath))
        {
            throw new GameNotFoundException();
        }

        try
        {
            string json = File.ReadAllText(saveFilePath);
            var data = JsonUtility.FromJson<GameStateData>(json);
            Debug.Log("[SaveSystem] Jogo carregado com sucesso.");
            return data;
        }
        catch (System.Exception ex)
        {
            throw new BadFormatGameException(ex.Message);
        }
    }

    public static List<string> GetAllSaveFileNames()
    {
        string savesDirectory = SaveFolder;
        if (!Directory.Exists(savesDirectory))
            return new List<string>();

        var files = Directory.GetFiles(savesDirectory, "*.json");

        for (int i = 0; i < files.Length; i++) files[i] = Path.GetFileName(files[i]);

        return files.ToList();
    }

    public static void DeleteSave(string saveFile)
    {
        var path = Path.Combine(SaveFolder, saveFile);

        if (File.Exists(path))
        {
            File.Delete(path);
        }
    }
}
