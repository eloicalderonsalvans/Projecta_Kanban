using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Projecta_Kanban
{
    public class Tasca : INotifyPropertyChanged
    {
        private string _nom;
        private string _descripcio;
        private string _estat;
        private Brush _backgroundColor;
        private bool _isSelected;
        private string _Autor;
        public DateTime DataInici { get; set; }
        public DateTime DataFinal { get; set; }

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

        public string Background { get; set; }

        [JsonIgnore]
        public Brush BackgroundColor
        {
            get { return _backgroundColor; }
            set { _backgroundColor = value; OnPropertyChanged(nameof(BackgroundColor)); }
        }

        [JsonIgnore]
        public bool IsSelected
        {
            get { return _isSelected; }
            set { _isSelected = value; OnPropertyChanged(nameof(IsSelected)); }
        }

        public string Autor
        {
            get { return _Autor; }
            set { _Autor = value; OnPropertyChanged(nameof(Autor)); }
        }


        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }



    }
}