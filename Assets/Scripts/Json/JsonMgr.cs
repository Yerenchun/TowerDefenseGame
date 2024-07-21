using UnityEngine;
using LitJson;
using System.IO;

/// <summary>
/// 序列化和反序列化时，使用的是哪种方案
/// </summary>
public enum JsonType
{
    JsonUtility,
    LitJson,
}

/// <summary>
/// Json数据管理 主要用于进行Json的序列化，存储到硬盘
/// 和反序列化，从硬盘中读取出来
/// </summary>
public class JsonMgr
{
    private static JsonMgr instance = new JsonMgr();
    public static JsonMgr Instance => instance;
    private JsonMgr(){}

    /// <summary>
    /// 存储数据
    /// </summary>
    /// <param name="data">被存储的数据</param>
    /// <param name="fileName">文件名</param>
    /// <param name="type">存储方案类型</param>
    public void SaveData(object data, string fileName, JsonType type = JsonType.LitJson)
    {
        // 确定存储路径
        string path = Application.persistentDataPath + "/" + fileName + ".json";
        string jsonStr = null;
        // 如果使用JsonUtility存储
        if(type == JsonType.JsonUtility)
        {
            jsonStr = JsonUtility.ToJson(data);
        }
        // 如果使用LitJson存储
        else if(type == JsonType.LitJson)
        {
            jsonStr = JsonMapper.ToJson(data);
        }
        // 写入文件中
        File.WriteAllText(path, jsonStr);
    }

    /// <summary>
    /// 读取数据
    /// 如果使用LitJson读取数据
    /// 被读取的数据必须需要有无参构造函数
    /// </summary>
    /// <param name="fileName">文件名</param>
    /// <param name="type">读取方案类型</param>
    /// <typeparam name="T">需要返回的类型</typeparam>
    /// <returns></returns>
    public T LoadData<T>(string fileName, JsonType type = JsonType.LitJson) where T : new()
    {
        // 确定存储路径
        // 首先判断 默认数据文件夹中，是否有想要的数据
        string path = Application.streamingAssetsPath + "/" + fileName + ".json";
        // 不存在这个文件，就从持久化数据文件夹中获取
        if(!File.Exists(path))
            path = Application.persistentDataPath + "/" + fileName + ".json";

        // 如果这个文件夹中，仍然没有，就返回一个默认对象
        if(!File.Exists(path))
            return new T();

        // 从文件中读取字符串
        string jsonStr = File.ReadAllText(path);

        T t = default(T);
        // 使用JsonUtility读取
        if(type == JsonType.JsonUtility)
        {
            t = JsonUtility.FromJson<T>(jsonStr);
        }
        else if(type == JsonType.LitJson)
        {
            t = JsonMapper.ToObject<T>(jsonStr);
        }

        return t;
    }

}
