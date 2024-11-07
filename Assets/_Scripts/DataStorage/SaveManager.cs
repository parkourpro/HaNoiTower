using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveManager
{
    private static string savePath = Path.Combine(Application.persistentDataPath, "levelData.json");

    // Lưu hoặc cập nhật dữ liệu của một level
    public static void SaveGameData(LevelData newLevelData)
    {
        // Tải tất cả dữ liệu hiện có
        List<LevelData> allLevelData = LoadGameData();

        // Tìm dữ liệu của level hiện tại
        LevelData existingLevel = allLevelData.Find(ld => ld.level == newLevelData.level);

        if (existingLevel != null)
        {
            // Nếu level đã tồn tại, so sánh bestTime và cập nhật nếu cần
            if (newLevelData.bestTime < existingLevel.bestTime)
            {
                existingLevel.stars = newLevelData.stars;
                existingLevel.bestTime = newLevelData.bestTime;
                Debug.Log("Updated existing level data.");
            }
        }
        else
        {
            // Nếu level chưa tồn tại, thêm mới dữ liệu
            allLevelData.Add(newLevelData);
            Debug.Log("Added new level data.");
        }

        // Lưu danh sách dữ liệu sau khi cập nhật hoặc thêm mới
        SaveAllGameData(allLevelData);
    }

    // Lưu toàn bộ danh sách LevelData thành JSON
    private static void SaveAllGameData(List<LevelData> allLevelData)
    {
        string json = JsonUtility.ToJson(new LevelDataWrapper(allLevelData));
        File.WriteAllText(savePath, json);
        Debug.Log("Game data saved to: " + savePath);
    }

    // Tải toàn bộ danh sách LevelData từ JSON
    public static List<LevelData> LoadGameData()
    {
        if (File.Exists(savePath))
        {
            try
            {
                string json = File.ReadAllText(savePath);

                // Kiểm tra nếu file trống hoặc không có nội dung hợp lệ
                if (string.IsNullOrEmpty(json))
                {
                    return new List<LevelData>();
                }

                // Chuyển đổi JSON thành đối tượng LevelDataWrapper
                LevelDataWrapper wrapper = JsonUtility.FromJson<LevelDataWrapper>(json);

                if (wrapper != null && wrapper.levelDataList != null)
                {
                    return wrapper.levelDataList;
                }
                else
                {
                    Debug.LogWarning("The save file is invalid or corrupted, returning an empty list.");
                    return new List<LevelData>();
                }
            }
            catch (System.Exception ex)
            {
                Debug.LogError("Error loading game data: " + ex.Message);
                return new List<LevelData>(); // Nếu có lỗi, trả về danh sách trống
            }
        }
        else
        {
            Debug.LogWarning("Save file not found, returning an empty list.");
            return new List<LevelData>();
        }
    }

    // Hàm xóa tất cả dữ liệu
    public static void DeleteAllGameData()
    {
        if (File.Exists(savePath))
        {
            try
            {
                File.Delete(savePath); // Xóa file lưu trữ
                Debug.Log("All game data deleted.");
            }
            catch (System.Exception ex)
            {
                Debug.LogError("Error deleting game data: " + ex.Message);
            }
        }
        else
        {
            Debug.LogWarning("Save file not found, nothing to delete.");
        }
    }
}

// Wrapper để bọc danh sách levelData
[System.Serializable]
public class LevelDataWrapper
{
    public List<LevelData> levelDataList;

    public LevelDataWrapper(List<LevelData> levelDataList)
    {
        this.levelDataList = levelDataList;
    }
}
