namespace Gestura.Commons
{
    /// <summary>
    /// Représente les différentes méthodes d'importation d'images de référence disponibles dans l'application.
    /// </summary>
    public enum ImportMethodEnum
    {
        /// <summary>
        /// État par défaut, aucune méthode d'importation n'est sélectionnée.
        /// </summary>
        None,
        /// <summary>
        /// Importer des images depuis le stockage local de l'appareil.
        /// </summary>
        LocalStorage,
        /// <summary>
        /// Importer des images depuis une URL Web (téléchargement direct).
        /// </summary>
        UrlWeb
    }
}
