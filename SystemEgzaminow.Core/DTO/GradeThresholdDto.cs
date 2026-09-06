using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SystemEgzaminow.Core.DTO
{
    public class GradeThresholdDto : INotifyPropertyChanged
    {
        public int? Id { get; set; }

        private decimal _ocena;

        public decimal Ocena
        {
            get => _ocena;
            set
            {
                _ocena = value;
                OnPropertyChanged();
            }
        }

        private decimal _progOd;

        public decimal ProgOd
        {
            get => _progOd;
            set
            {
                _progOd = value;
                OnPropertyChanged();
            }
        }

        private decimal _progDo;

        public decimal ProgDo
        {
            get => _progDo;
            set
            {
                _progDo = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}