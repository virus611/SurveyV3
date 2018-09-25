
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
[ORM(Name = "B_RspdAnswer")]
public partial class RspdAnswer : INotifyPropertyChanged
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

public RspdAnswer()
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

private string m_RID;
/// <summary>
/// 
/// </summary>
[ORM(DBType = DbType.String, Size = 50, Name = "RID_nvarchar")]
public string RID
{
set
{ 
m_RID = value;
PropertyChanged(this, new PropertyChangedEventArgs("RID"));
}
get { return m_RID; }
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

private string m_QNO;
/// <summary>
/// 
/// </summary>
[ORM(DBType = DbType.String, Size = 50, Name = "QNO_nvarchar")]
public string QNO
{
set
{ 
m_QNO = value;
PropertyChanged(this, new PropertyChangedEventArgs("QNO"));
}
get { return m_QNO; }
}

private string m_Item;
/// <summary>
/// 
/// </summary>
[ORM(DBType = DbType.String, Size = 50, Name = "Item_nvarchar")]
public string Item
{
set
{ 
m_Item = value;
PropertyChanged(this, new PropertyChangedEventArgs("Item"));
}
get { return m_Item; }
}

private bool? m_DelFlag;
/// <summary>
/// 
/// </summary>
[ORM(DBType = DbType.Boolean, Size = 1, Name = "DelFlag_bit")]
public bool? DelFlag
{
set
{ 
m_DelFlag = value;
PropertyChanged(this, new PropertyChangedEventArgs("DelFlag"));
}
get { return m_DelFlag; }
}
}
}
