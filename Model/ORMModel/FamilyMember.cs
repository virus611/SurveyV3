
using ORM;
using System;
using System.Data;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;//[IgnoreDataMember]
using System.Web.Script.Serialization;//[ScriptIgnore]

namespace Model.ORMModel
{
/// <summary>
/// 
/// </summary>
[ORM(Name = "B_FamilyMember")]
public partial class FamilyMember : INotifyPropertyChanged
{

//传递属性变化通知
public event PropertyChangedEventHandler PropertyChanged;
/// <summary>
/// 添加UpdateColumns记录
/// </summary>
private void AddUpdateColumns(object sender, PropertyChangedEventArgs e)
{
if (m_updatecolumns == null) m_updatecolumns = new List<string>();
if (m_updatecolumns.Contains(e.PropertyName) == false) m_updatecolumns.Add(e.PropertyName);
}

public FamilyMember()
{
//注册事件
PropertyChanged += AddUpdateColumns;
}

private List<string> m_updatecolumns;
/// <summary>
/// 更新列集合
/// </summary>
[ScriptIgnore]//使用JavaScriptSerializer序列化时不序列化此字段
[IgnoreDataMember]//使用DataContractJsonSerializer序列化时不序列化此字段
//[JsonIgnore]//使用JsonConvert序列化时不序列化此字段
public List<string> UpdateColumns
{
set { m_updatecolumns = value; }
get { return m_updatecolumns; }
}

private string m_ID;
/// <summary>
/// 
/// </summary>
[ORM(DBType = DbType.String, Size = 50, Name = "ID_nvarchar", IsKey = true)]
public string ID
{
set
{ 
m_ID = value;
PropertyChanged(this, new PropertyChangedEventArgs("ID"));
}
get { return m_ID; }
}

private string m_Code;
/// <summary>
/// 
/// </summary>
[ORM(DBType = DbType.String, Size = 50, Name = "Code_nvarchar")]
public string Code
{
set
{ 
m_Code = value;
PropertyChanged(this, new PropertyChangedEventArgs("Code"));
}
get { return m_Code; }
}

private string m_Name;
/// <summary>
/// 
/// </summary>
[ORM(DBType = DbType.String, Size = 50, Name = "Name_nvarchar")]
public string Name
{
set
{ 
m_Name = value;
PropertyChanged(this, new PropertyChangedEventArgs("Name"));
}
get { return m_Name; }
}



private string m_FID;
/// <summary>
/// 
/// </summary>
[ORM(DBType = DbType.String, Size = 50, Name = "FID_nvarchar")]
public string FID
{
    set
    {
        m_FID = value;
        PropertyChanged(this, new PropertyChangedEventArgs("FID"));
    }
    get { return m_FID; }
}

private int? m_Sex;
/// <summary>
/// 
/// </summary>
[ORM(DBType = DbType.Int32, Size = 4, Name = "Sex_int")]
public int? Sex
{
set
{ 
m_Sex = value;
PropertyChanged(this, new PropertyChangedEventArgs("Sex"));
}
get { return m_Sex; }
}

private int? m_Age;
/// <summary>
/// 
/// </summary>
[ORM(DBType = DbType.Int32, Size = 4, Name = "Age_int")]
public int? Age
{
set
{ 
m_Age = value;
PropertyChanged(this, new PropertyChangedEventArgs("Age"));
}
get { return m_Age; }
}
}
}
