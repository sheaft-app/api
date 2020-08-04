# Sheaft API (utilisée par https://app.sheaft.com)

Ce projet permet de prendre en compte les requêtes de l'interface web de Sheaft à l'aide de GraphQL et dotnetcore

## Pré-requis

- Entity Framework Core CLI: https://docs.microsoft.com/en-us/ef/core/miscellaneous/cli/dotnet 
- Un container SQL docker avec l'image suivante: "docker run --name app -e 'ACCEPT_EULA=Y' -e 'SA_PASSWORD=##REPLACE##' -p 1434:1433 -d mcr.microsoft.com/mssql/server:2019-latest". Le port docker est redirigé sur 1434 pour ne pas interférer avec un serveur SQL déjà présent sur la machine.
- SQLLocalDb/Express si vous possédez déjà une instance configurée et ne souhaitez pas utiliser docker (pensez à mettre à jour le port dans le fichier appsettings.json).
- dotnet core 3.1 : https://dotnet.microsoft.com/download/dotnet-core/3.1
- Un compte Sendgrid: https://sendgrid.com/pricing (il faudra créer des templates, et renseigner les ids dans les events correspondant dans la partie Sheaft.Application.Events).
- Un compte INSEE: https://api.insee.fr/ pour permettre d'effectuer les recherches de SIRET lors de l'enregistrement d'un compte producteur ou magasin.
- Un serveur d'identité: le code source de celui de Sheaft est disponible dans le repository https://github.com/sheaft-app/identity, il démarre par défaut sur https://localhost:5001
- Un compte de stockage Azure sur lequel sera chargé les images des produits, les exports/imports et autres données requises.
- Un serveur Signalr pour envoyer les notifications de changement d'états à la partie Web, le code est disponible dans Sheaft.Signalr (il faut lui configurer une clé d'API pour pouvoir l'appeler, le setting est signalr:apikey).
- Une plateforme Azure Functions pour executer les traitements asynchrones (export, notification, emailing etc), le code est disponible dans Sheaft.Functions

## API (Sheaft.Api)

Le serveur d'api utilise AspNetCore 3.1 pour fournir le point d'entré applicatif.

## GraphQL (Sheaft.GraphQL)

Les données sont exposées et manipulées à l'aide d'une API GraphQL. la documentation est disponible en installant Altaïr (https://altair.sirmuel.design) et en vous renseignant l'adresse https://localhost:5003/graphql.

L'implémentation est faite à l'aide du framework HotChocolate https://hotchocolate.io/docs/introduction

## Orchestration (Sheaft.Application.xxxx)

L'API est orchestrée à l'aide du framework Mediatr, plus d'informations sont disponibles à cette adresse: https://github.com/jbogard/MediatR, nos mutations GraphQL appellent des Commandes Mediatr (Sheaft.Application.Commands) et lèvent ou non des Events (Sheaft.Application.Events). Ces commandes et events sont traités dans des Handlers dédiés (Sheaft.Application.Handlers). La lecture de données se fait via une couche Queries (Sheaft.Application.Queries) pour séparer la notion de lecture de la modification.

## Transformation des données (Sheaft.Mappers)

Les données (Input) sont automatiquement converties en commandes (Mediatr) à l'aide d'AutoMapper: https://docs.automapper.org/en/latest/.
Les données (EF) sont automatiquement converties en DTO via AutoMapper dans la couche de Queries (Sheaft.Application.Queries) en utilisant la méthode ProjectTo<T>().

Cela nous permet de sélectionner uniquement les données nécessaires SQL lors de l'execution des requêtes GraphQL et minimise ainsi la quantité de données qui transite.

## DDD (Sheaft.Domain)

Les modèles applicatifs sont implémentés en suivant le pattern DDD, encapsulant la majeure partie des traitements. Les modifications d'état sont donc portées par l'objet lui même, plutôt que par un service : https://enterprisecraftsmanship.com/posts/new-online-course-ddd-and-ef-core/

## Base de données (Sheaft.Infrastructure)

La base de données est utilisée via l'ORM EntityFrameworkCore, cette couche utilise directement Sheaft.Domains pour mapper les tables SQL aux classes, aucune clé de liaison n'est exposée pour que le modèle reste "pur" de toute spécification propre à l'infrastructure. Cela est possible grace au shadow mapping apporté par EFCore : https://docs.microsoft.com/en-us/ef/core/modeling/shadow-properties

## Evolution du modèle de base de données

Pour la mettre à jour le modèle de données, nous utilisons les migrations d'EF. 
Il faut donc faire les modifications nécessaires sur AppDbContext puis se placer dans le répertoire Sheaft.Infrastructure et executer:  dotnet-ef migrations add ##REPLACE## -s ..\Sheaft.Api\Sheaft.Api.csproj -c AppDbContext 

Vous pouvez ensuite appliquer la migration à l'aide de la commande suivante: dotnet-ef database update ##REPLACE###

Vous pouvez annuler la dernière migration si celle-ci n'a pas été appliquée via:  dotnet-ef migrations remove -s ..\Sheaft.Api\Sheaft.Api.csproj -c AppDbContext 

L'utilisation du path vers le projet web est nécessaire pour executer la commande.