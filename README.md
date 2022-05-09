# Sheaft API

Ce projet permet de prendre en compte les requêtes de l'interface web de Sheaft

## Pré-requis

- dotnet 6.0 : https://dotnet.microsoft.com/download/dotnet/6.0
- Entity Framework Core CLI: https://docs.microsoft.com/en-us/ef/core/miscellaneous/cli/dotnet 
- Un container SQL docker avec l'image suivante: "docker run --name app -e 'ACCEPT_EULA=Y' -e 'SA_PASSWORD=##REPLACE##' -p 1434:1433 -d mcr.microsoft.com/mssql/server:2019-latest". Le port docker est redirigé sur 1434 pour ne pas interférer avec un serveur SQL déjà présent sur la machine.
- SQLLocalDb/Express si vous possédez déjà une instance configurée et ne souhaitez pas utiliser docker (pensez à mettre à jour le port dans le fichier appsettings.json).
- Un compte Amazon SES: https://aws.amazon.com/fr/ses/
- Un compte INSEE: https://api.insee.fr/ pour permettre d'effectuer les recherches de SIRET lors de l'enregistrement d'un compte producteur ou magasin.
- Un compte de stockage Azure sur lequel sera chargé les images des produits, les exports/imports et autres données requises.

## Evolution du modèle de base de données

Pour mettre à jour le modèle de données, nous utilisons les migrations d'EF.
Il faut donc faire les modifications nécessaires sur AppDbContext puis se placer dans le répertoire Sheaft.Infrastructure et executer:  dotnet-ef migrations add ##REPLACE## -s ../Sheaft.Web.Api/Sheaft.Web.Api.csproj -c AppDbContext

Vous pouvez ensuite appliquer la migration à l'aide de la commande suivante: dotnet-ef database update ##REPLACE###

Vous pouvez annuler la dernière migration si celle-ci n'a pas été appliquée via:  dotnet-ef migrations remove -s ../Sheaft.Web.Api/Sheaft.Web.Api.csproj -c AppDbContext

L'utilisation du path vers le projet web est nécessaire pour executer la commande.
