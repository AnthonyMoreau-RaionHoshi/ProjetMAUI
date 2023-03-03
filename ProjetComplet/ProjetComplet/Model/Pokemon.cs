namespace ProjetComplet.Model;

public class Pokemon: ContentPage
{
    public string id { get; set; }
    public string name_english { get;  set; }
    public string name_japanese { get;  set; }
    public string name_chinese { get;  set; }
    public string name_french { get;  set; }
    public string type_0 { get;  set; }
    public string type_1 { get;  set; }
    public string base_HP { get;  set; }
    public string base_Attack { get;  set; }
    public string base_Defense { get;  set; }
    public string base_Sp_Attack { get;  set; }
    public string base_Sp_Defense { get;  set; }
    public string base_Speed { get;  set; }
    public string Picture { get;  set; }
}