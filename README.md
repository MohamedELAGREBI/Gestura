# Gestura

## Présentation
**Gestura** est une application .NET MAUI pensée pour aider les artistes et passionnés de dessin à **s’amuser** tout en améliorant leurs **gestures** (croquis rapides). L’idée est simple : vous sélectionnez des images de référence et l’app bascule automatiquement d’une image à l’autre après un certain temps. De quoi **s’éclater** en variant les poses et en travaillant son coup de crayon !

### Fonctionnalités clés
- **Définir le temps entre chaque pose** : Ajustez facilement l’intervalle (quelques secondes ou minutes) avant de découvrir la prochaine image.  
- **Choisir ses images** :  
  - Importer depuis la galerie ou la mémoire de votre appareil.  
  - Coller l’URL d’une image pour la télécharger directement.  
- **Créer plusieurs sessions** : Gérez vos différentes séances (liste d’images, durée, etc.) pour une **expérience personnalisée** et toujours plus de **fun**.

### Pour qui ?
- Artistes (professionnels ou amateurs), étudiants en écoles d’art…  
- Toute personne souhaitant **pimenter** ses séances de dessin ou simplement **s’amuser** à croquer des poses en rafale.

---

## Installation
1. **Prérequis** :  
   - .NET 7 (ou version compatible .NET MAUI)  
   - Visual Studio 2022 (ou équivalent) avec la charge de travail « .NET Multi-platform App UI development ».  
2. **Cloner le dépôt** :  
   ```bash
   git clone https://github.com/votre-compte/Gestura.git
   cd Gestura
3. **Restaurer les packages NuGet** :
   ```bash
   dotnet restore
4. **Exécuter l’application** :
   - **Avec Visual Studio** : Ouvrez la solution .sln, puis appuyez sur F5.
   - **En ligne de commande** :
   ```bash
   dotnet build
   dotnet run
  - Sélectionnez la plateforme souhaitée (Android, iOS, Windows…).

---

## Utilisation
1. **Importer des images** :
   - **Depuis l’appareil** : Parcourez vos dossiers pour ajouter vos clichés préférés.
   - **Depuis une URL** : Copiez simplement le lien de l’image et collez-le dans Gestura.
2. **Créer une session** : Donnez-lui un nom et réglez la durée entre chaque pose.
3. **Lancer la session** : Le défilé commence ! L’application fera apparaître chaque image selon l’intervalle défini.
4. **Interactivité** : Mettez en pause pour prendre plus de temps, ou zappez une image si vous êtes pressé de découvrir la suivante.
 
---

## Roadmap / Évolutions futures
- **Import depuis des Drives** (Google Drive, Dropbox, etc.) pour récupérer directement vos collections.
- **Intégration de plateformes d’inspiration** (Pinterest, Tumblr, etc.).
- **Catégorisation / Tag** : Organiser vos images par thèmes (paysages, portraits, anatomie…).
- **Personnalisation de l’interface** : Thèmes colorés, mode clair/sombre, etc.
- **Statistiques** : Comptabiliser le nombre de poses dessinées, le temps total passé… pour célébrer vos progrès !
- **Internationalisation** : Permettre l'adaption de l'application à la langue de l'appareil.
- **Préférences utilisateurs** : Gestion des préférences utilisateur de l'application.

---

## Remarques finales
- Technologies : .NET MAUI (C#), compatible multi-plateformes (Android, iOS, Windows…).
- Objectif : Rendre vos séances de dessin plus ludiques, plus dynamiques et surtout… plus créatives !

Amusez-vous bien avec Gestura, et laissez libre cours à votre imagination !
