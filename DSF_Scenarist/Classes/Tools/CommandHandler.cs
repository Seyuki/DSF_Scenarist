using System;
using System.Windows.Input;

namespace DSF_Scenarist.Classes.Tools
{
    /// <summary>
    /// Handler de commande avec paramètre
    /// </summary>
    public class CommandHandler : ICommand
    {
        #region Propriétés

        /// <summary>
        /// Action de la commande (methode)
        /// </summary>
        private Action<object> _action;

        /// <summary>
        /// Flag indiquant si la commande peut être executée
        /// </summary>
        private bool _canExecute;

        #endregion Propriétés

        #region Constructeur

        /// <summary>
        /// Constructeur par défaut
        /// </summary>
        /// <param name="action">Action de la commande (methode)</param>
        /// <param name="canExecute">Flag indiquant si la commande peut être executée</param>
        public CommandHandler(Action<object> action, bool canExecute)
        {
            _action = action;
            _canExecute = canExecute;
        }

        #endregion Constructeur

        #region Methodes

        /// <summary>
        /// Evenement de changement d'état sur la possibilité d'éxecuter la commande
        /// </summary>
        public void RaiseCanExecuteChanged()
        {
            if (CanExecuteChanged != null)
                CanExecuteChanged(this, new EventArgs());
        }

        /// <summary>
        /// Evenement de changement d'état sur la possibilité d'éxecuter la commande
        /// </summary>
        public event EventHandler CanExecuteChanged;

        /// <summary>
        /// Methode permettant de savoir si la command peut être executée
        /// </summary>
        /// <param name="parameter">Paramètre de la commande</param>
        /// <returns>true si la commande peut être executée, sinon false</returns>
        public bool CanExecute(object parameter)
        {
            return _canExecute;
        }

        /// <summary>
        /// Execution de la commande
        /// </summary>
        /// <param name="parameter">Paramètre à envoyer à la methode associée à la commande</param>
        public void Execute(object parameter)
        {
            _action(parameter);
        }

        #endregion Methodes
    }
}
