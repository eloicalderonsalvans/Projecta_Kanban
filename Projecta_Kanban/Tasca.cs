using System.ComponentModel;
using System.Windows.Media;

public class Tasca : INotifyPropertyChanged
{
    private string _nom;
    private string _descripcio;
    private string _estat;
    private Brush _background;
    private bool _isSelected;

    public event PropertyChangedEventHandler PropertyChanged;

    public string Nom
    {
        get { return _nom; }
        set { _nom = value; OnPropertyChanged(nameof(Nom)); }
    }

    public string Descripcio
    {
        get { return _descripcio; }
        set { _descripcio = value; OnPropertyChanged(nameof(Descripcio)); }
    }

    public string Estat
    {
        get { return _estat; }
        set { _estat = value; OnPropertyChanged(nameof(Estat)); }
    }

    public Brush Background
    {
        get { return _background; }
        set { _background = value; OnPropertyChanged(nameof(Background)); }
    }

    public bool IsSelected
    {
        get { return _isSelected; }
        set { _isSelected = value; OnPropertyChanged(nameof(IsSelected)); }
    }

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
