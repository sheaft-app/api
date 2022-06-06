
/**
 * @roxi/routify 2.18.5
 * File generated Mon Jun 06 2022 22:37:51 GMT+0200 (heure d’été d’Europe centrale)
 */

export const __version = "2.18.5"
export const __timestamp = "2022-06-06T20:37:51.526Z"

//buildRoutes
import { buildClientTree } from "@roxi/routify/runtime/buildRoutes"

//imports


//options
export const options = {}

//tree
export const _tree = {
  "name": "_layout",
  "filepath": "/_layout.svelte",
  "root": true,
  "ownMeta": {},
  "absolutePath": "D:/Projects/Sheaft/api/Sheaft.Web.Portal/src/pages/_layout.svelte",
  "children": [
    {
      "isFile": true,
      "isDir": false,
      "file": "_fallback.svelte",
      "filepath": "/_fallback.svelte",
      "name": "_fallback",
      "ext": "svelte",
      "badExt": false,
      "absolutePath": "D:/Projects/Sheaft/api/Sheaft.Web.Portal/src/pages/_fallback.svelte",
      "importPath": "../src/pages/_fallback.svelte",
      "isLayout": false,
      "isReset": false,
      "isIndex": false,
      "isFallback": true,
      "isPage": false,
      "ownMeta": {},
      "meta": {
        "recursive": true,
        "preload": false,
        "prerender": true
      },
      "path": "/_fallback",
      "id": "__fallback",
      "component": () => import('../src/pages/_fallback.svelte').then(m => m.default)
    },
    {
      "isFile": false,
      "isDir": true,
      "file": "auth",
      "filepath": "/auth",
      "name": "auth",
      "ext": "",
      "badExt": false,
      "absolutePath": "D:/Projects/Sheaft/api/Sheaft.Web.Portal/src/pages/auth",
      "children": [
        {
          "isFile": true,
          "isDir": false,
          "file": "login.svelte",
          "filepath": "/auth/login.svelte",
          "name": "login",
          "ext": "svelte",
          "badExt": false,
          "absolutePath": "D:/Projects/Sheaft/api/Sheaft.Web.Portal/src/pages/auth/login.svelte",
          "importPath": "../src/pages/auth/login.svelte",
          "isLayout": false,
          "isReset": false,
          "isIndex": false,
          "isFallback": false,
          "isPage": true,
          "ownMeta": {
            "anonymous": true,
            "title": "Bienvenue"
          },
          "meta": {
            "anonymous": true,
            "title": "Bienvenue",
            "recursive": true,
            "preload": false,
            "prerender": true
          },
          "path": "/auth/login",
          "id": "_auth_login",
          "component": () => import('../src/pages/auth/login.svelte').then(m => m.default)
        },
        {
          "isFile": true,
          "isDir": false,
          "file": "forgot.svelte",
          "filepath": "/auth/forgot.svelte",
          "name": "forgot",
          "ext": "svelte",
          "badExt": false,
          "absolutePath": "D:/Projects/Sheaft/api/Sheaft.Web.Portal/src/pages/auth/forgot.svelte",
          "importPath": "../src/pages/auth/forgot.svelte",
          "isLayout": false,
          "isReset": false,
          "isIndex": false,
          "isFallback": false,
          "isPage": true,
          "ownMeta": {
            "anonymous": true,
            "title": "J'ai oublié mon mot de passe"
          },
          "meta": {
            "anonymous": true,
            "title": "J'ai oublié mon mot de passe",
            "recursive": true,
            "preload": false,
            "prerender": true
          },
          "path": "/auth/forgot",
          "id": "_auth_forgot",
          "component": () => import('../src/pages/auth/forgot.svelte').then(m => m.default)
        },
        {
          "isFile": true,
          "isDir": false,
          "file": "reset.svelte",
          "filepath": "/auth/reset.svelte",
          "name": "reset",
          "ext": "svelte",
          "badExt": false,
          "absolutePath": "D:/Projects/Sheaft/api/Sheaft.Web.Portal/src/pages/auth/reset.svelte",
          "importPath": "../src/pages/auth/reset.svelte",
          "isLayout": false,
          "isReset": false,
          "isIndex": false,
          "isFallback": false,
          "isPage": true,
          "ownMeta": {
            "anonymous": true,
            "title": "Modifier votre mot de passe"
          },
          "meta": {
            "anonymous": true,
            "title": "Modifier votre mot de passe",
            "recursive": true,
            "preload": false,
            "prerender": true
          },
          "path": "/auth/reset",
          "id": "_auth_reset",
          "component": () => import('../src/pages/auth/reset.svelte').then(m => m.default)
        },
        {
          "isFile": true,
          "isDir": false,
          "file": "configure.svelte",
          "filepath": "/auth/configure.svelte",
          "name": "configure",
          "ext": "svelte",
          "badExt": false,
          "absolutePath": "D:/Projects/Sheaft/api/Sheaft.Web.Portal/src/pages/auth/configure.svelte",
          "importPath": "../src/pages/auth/configure.svelte",
          "isLayout": false,
          "isReset": false,
          "isIndex": false,
          "isFallback": false,
          "isPage": true,
          "ownMeta": {
            "title": "Renseigner vos informations"
          },
          "meta": {
            "title": "Renseigner vos informations",
            "recursive": true,
            "preload": false,
            "prerender": true
          },
          "path": "/auth/configure",
          "id": "_auth_configure",
          "component": () => import('../src/pages/auth/configure.svelte').then(m => m.default)
        },
        {
          "isFile": true,
          "isDir": false,
          "file": "register.svelte",
          "filepath": "/auth/register.svelte",
          "name": "register",
          "ext": "svelte",
          "badExt": false,
          "absolutePath": "D:/Projects/Sheaft/api/Sheaft.Web.Portal/src/pages/auth/register.svelte",
          "importPath": "../src/pages/auth/register.svelte",
          "isLayout": false,
          "isReset": false,
          "isIndex": false,
          "isFallback": false,
          "isPage": true,
          "ownMeta": {
            "anonymous": true,
            "title": "Renseigner vos informations"
          },
          "meta": {
            "anonymous": true,
            "title": "Renseigner vos informations",
            "recursive": true,
            "preload": false,
            "prerender": true
          },
          "path": "/auth/register",
          "id": "_auth_register",
          "component": () => import('../src/pages/auth/register.svelte').then(m => m.default)
        }
      ],
      "isLayout": false,
      "isReset": false,
      "isIndex": false,
      "isFallback": false,
      "isPage": false,
      "ownMeta": {},
      "meta": {
        "recursive": true,
        "preload": false,
        "prerender": true
      },
      "path": "/auth"
    },
    {
      "isFile": true,
      "isDir": false,
      "file": "unauthorized.svelte",
      "filepath": "/unauthorized.svelte",
      "name": "unauthorized",
      "ext": "svelte",
      "badExt": false,
      "absolutePath": "D:/Projects/Sheaft/api/Sheaft.Web.Portal/src/pages/unauthorized.svelte",
      "importPath": "../src/pages/unauthorized.svelte",
      "isLayout": false,
      "isReset": false,
      "isIndex": false,
      "isFallback": false,
      "isPage": true,
      "ownMeta": {
        "public": true,
        "index": true,
        "title": "Accès non autorisé"
      },
      "meta": {
        "public": true,
        "index": true,
        "title": "Accès non autorisé",
        "recursive": true,
        "preload": false,
        "prerender": true
      },
      "path": "/unauthorized",
      "id": "_unauthorized",
      "component": () => import('../src/pages/unauthorized.svelte').then(m => m.default)
    },
    {
      "isFile": true,
      "isDir": false,
      "file": "index.svelte",
      "filepath": "/index.svelte",
      "name": "index",
      "ext": "svelte",
      "badExt": false,
      "absolutePath": "D:/Projects/Sheaft/api/Sheaft.Web.Portal/src/pages/index.svelte",
      "importPath": "../src/pages/index.svelte",
      "isLayout": false,
      "isReset": false,
      "isIndex": true,
      "isFallback": false,
      "isPage": true,
      "ownMeta": {
        "public": true,
        "index": 1,
        "menu": "Accueil",
        "title": "Bienvenue sur votre dashboard",
        "icon": "fas#desktopAlt"
      },
      "meta": {
        "public": true,
        "index": 1,
        "menu": "Accueil",
        "title": "Bienvenue sur votre dashboard",
        "icon": "fas#desktopAlt",
        "recursive": true,
        "preload": false,
        "prerender": true
      },
      "path": "/index",
      "id": "_index",
      "component": () => import('../src/pages/index.svelte').then(m => m.default)
    },
    {
      "isFile": true,
      "isDir": true,
      "file": "_layout.svelte",
      "filepath": "/products/_layout.svelte",
      "name": "_layout",
      "ext": "svelte",
      "badExt": false,
      "absolutePath": "D:/Projects/Sheaft/api/Sheaft.Web.Portal/src/pages/products/_layout.svelte",
      "children": [
        {
          "isFile": true,
          "isDir": false,
          "file": "[id].svelte",
          "filepath": "/products/[id].svelte",
          "name": "[id]",
          "ext": "svelte",
          "badExt": false,
          "absolutePath": "D:/Projects/Sheaft/api/Sheaft.Web.Portal/src/pages/products/[id].svelte",
          "importPath": "../src/pages/products/[id].svelte",
          "isLayout": false,
          "isReset": false,
          "isIndex": false,
          "isFallback": false,
          "isPage": true,
          "ownMeta": {
            "index": true,
            "title": "Details du produit",
            "roles": []
          },
          "meta": {
            "index": true,
            "title": "Details du produit",
            "roles": [],
            "recursive": true,
            "preload": false,
            "prerender": true
          },
          "path": "/products/:id",
          "id": "_products__id",
          "component": () => import('../src/pages/products/[id].svelte').then(m => m.default)
        },
        {
          "isFile": true,
          "isDir": false,
          "file": "index.svelte",
          "filepath": "/products/index.svelte",
          "name": "index",
          "ext": "svelte",
          "badExt": false,
          "absolutePath": "D:/Projects/Sheaft/api/Sheaft.Web.Portal/src/pages/products/index.svelte",
          "importPath": "../src/pages/products/index.svelte",
          "isLayout": false,
          "isReset": false,
          "isIndex": true,
          "isFallback": false,
          "isPage": true,
          "ownMeta": {
            "menu": "Mes produits",
            "title": "Mes produits",
            "roles": [],
            "index": 1,
            "default": true
          },
          "meta": {
            "menu": "Mes produits",
            "title": "Mes produits",
            "roles": [],
            "index": 1,
            "default": true,
            "recursive": true,
            "preload": false,
            "prerender": true
          },
          "path": "/products/index",
          "id": "_products_index",
          "component": () => import('../src/pages/products/index.svelte').then(m => m.default)
        },
        {
          "isFile": true,
          "isDir": false,
          "file": "create.svelte",
          "filepath": "/products/create.svelte",
          "name": "create",
          "ext": "svelte",
          "badExt": false,
          "absolutePath": "D:/Projects/Sheaft/api/Sheaft.Web.Portal/src/pages/products/create.svelte",
          "importPath": "../src/pages/products/create.svelte",
          "isLayout": false,
          "isReset": false,
          "isIndex": false,
          "isFallback": false,
          "isPage": true,
          "ownMeta": {
            "menu": "Créer",
            "index": 2,
            "title": "Ajouter un nouveau produit",
            "icon": "fas#coffee"
          },
          "meta": {
            "menu": "Créer",
            "index": 2,
            "title": "Ajouter un nouveau produit",
            "icon": "fas#coffee",
            "recursive": true,
            "preload": false,
            "prerender": true
          },
          "path": "/products/create",
          "id": "_products_create",
          "component": () => import('../src/pages/products/create.svelte').then(m => m.default)
        }
      ],
      "isLayout": true,
      "isReset": false,
      "isIndex": false,
      "isFallback": false,
      "isPage": false,
      "importPath": "../src/pages/products/_layout.svelte",
      "ownMeta": {
        "menu": "Produits",
        "index": 2,
        "group": true,
        "icon": "fas#barcode"
      },
      "meta": {
        "menu": "Produits",
        "index": 2,
        "group": true,
        "icon": "fas#barcode",
        "recursive": true,
        "preload": false,
        "prerender": true
      },
      "path": "/products",
      "id": "_products__layout",
      "component": () => import('../src/pages/products/_layout.svelte').then(m => m.default)
    },
    {
      "isFile": true,
      "isDir": true,
      "file": "_layout.svelte",
      "filepath": "/returnables/_layout.svelte",
      "name": "_layout",
      "ext": "svelte",
      "badExt": false,
      "absolutePath": "D:/Projects/Sheaft/api/Sheaft.Web.Portal/src/pages/returnables/_layout.svelte",
      "children": [
        {
          "isFile": true,
          "isDir": false,
          "file": "[id].svelte",
          "filepath": "/returnables/[id].svelte",
          "name": "[id]",
          "ext": "svelte",
          "badExt": false,
          "absolutePath": "D:/Projects/Sheaft/api/Sheaft.Web.Portal/src/pages/returnables/[id].svelte",
          "importPath": "../src/pages/returnables/[id].svelte",
          "isLayout": false,
          "isReset": false,
          "isIndex": false,
          "isFallback": false,
          "isPage": true,
          "ownMeta": {
            "index": true,
            "title": "Details du produit",
            "roles": []
          },
          "meta": {
            "index": true,
            "title": "Details du produit",
            "roles": [],
            "recursive": true,
            "preload": false,
            "prerender": true
          },
          "path": "/returnables/:id",
          "id": "_returnables__id",
          "component": () => import('../src/pages/returnables/[id].svelte').then(m => m.default)
        },
        {
          "isFile": true,
          "isDir": false,
          "file": "index.svelte",
          "filepath": "/returnables/index.svelte",
          "name": "index",
          "ext": "svelte",
          "badExt": false,
          "absolutePath": "D:/Projects/Sheaft/api/Sheaft.Web.Portal/src/pages/returnables/index.svelte",
          "importPath": "../src/pages/returnables/index.svelte",
          "isLayout": false,
          "isReset": false,
          "isIndex": true,
          "isFallback": false,
          "isPage": true,
          "ownMeta": {
            "menu": "Mes consignes",
            "title": "Mes consignes",
            "roles": [],
            "index": 1,
            "default": true
          },
          "meta": {
            "menu": "Mes consignes",
            "title": "Mes consignes",
            "roles": [],
            "index": 1,
            "default": true,
            "recursive": true,
            "preload": false,
            "prerender": true
          },
          "path": "/returnables/index",
          "id": "_returnables_index",
          "component": () => import('../src/pages/returnables/index.svelte').then(m => m.default)
        },
        {
          "isFile": true,
          "isDir": false,
          "file": "create.svelte",
          "filepath": "/returnables/create.svelte",
          "name": "create",
          "ext": "svelte",
          "badExt": false,
          "absolutePath": "D:/Projects/Sheaft/api/Sheaft.Web.Portal/src/pages/returnables/create.svelte",
          "importPath": "../src/pages/returnables/create.svelte",
          "isLayout": false,
          "isReset": false,
          "isIndex": false,
          "isFallback": false,
          "isPage": true,
          "ownMeta": {
            "menu": "Créer",
            "index": 2,
            "title": "Ajouter une nouvelle consigne",
            "icon": "fas#coffee"
          },
          "meta": {
            "menu": "Créer",
            "index": 2,
            "title": "Ajouter une nouvelle consigne",
            "icon": "fas#coffee",
            "recursive": true,
            "preload": false,
            "prerender": true
          },
          "path": "/returnables/create",
          "id": "_returnables_create",
          "component": () => import('../src/pages/returnables/create.svelte').then(m => m.default)
        }
      ],
      "isLayout": true,
      "isReset": false,
      "isIndex": false,
      "isFallback": false,
      "isPage": false,
      "importPath": "../src/pages/returnables/_layout.svelte",
      "ownMeta": {
        "menu": "Consignes",
        "index": 3,
        "group": true,
        "icon": "fas#barcode"
      },
      "meta": {
        "menu": "Consignes",
        "index": 3,
        "group": true,
        "icon": "fas#barcode",
        "recursive": true,
        "preload": false,
        "prerender": true
      },
      "path": "/returnables",
      "id": "_returnables__layout",
      "component": () => import('../src/pages/returnables/_layout.svelte').then(m => m.default)
    },
    {
      "isFile": true,
      "isDir": true,
      "file": "_layout.svelte",
      "filepath": "/orders/_layout.svelte",
      "name": "_layout",
      "ext": "svelte",
      "badExt": false,
      "absolutePath": "D:/Projects/Sheaft/api/Sheaft.Web.Portal/src/pages/orders/_layout.svelte",
      "children": [
        {
          "isFile": true,
          "isDir": false,
          "file": "index.svelte",
          "filepath": "/orders/index.svelte",
          "name": "index",
          "ext": "svelte",
          "badExt": false,
          "absolutePath": "D:/Projects/Sheaft/api/Sheaft.Web.Portal/src/pages/orders/index.svelte",
          "importPath": "../src/pages/orders/index.svelte",
          "isLayout": false,
          "isReset": false,
          "isIndex": true,
          "isFallback": false,
          "isPage": true,
          "ownMeta": {
            "menu": "Mes commandes",
            "title": "Commandes en cours",
            "roles": [],
            "index": 1,
            "default": true
          },
          "meta": {
            "menu": "Mes commandes",
            "title": "Commandes en cours",
            "roles": [],
            "index": 1,
            "default": true,
            "recursive": true,
            "preload": false,
            "prerender": true
          },
          "path": "/orders/index",
          "id": "_orders_index",
          "component": () => import('../src/pages/orders/index.svelte').then(m => m.default)
        },
        {
          "isFile": true,
          "isDir": false,
          "file": "[id].svelte",
          "filepath": "/orders/[id].svelte",
          "name": "[id]",
          "ext": "svelte",
          "badExt": false,
          "absolutePath": "D:/Projects/Sheaft/api/Sheaft.Web.Portal/src/pages/orders/[id].svelte",
          "importPath": "../src/pages/orders/[id].svelte",
          "isLayout": false,
          "isReset": false,
          "isIndex": false,
          "isFallback": false,
          "isPage": true,
          "ownMeta": {
            "index": true,
            "title": "Details de la commande",
            "roles": []
          },
          "meta": {
            "index": true,
            "title": "Details de la commande",
            "roles": [],
            "recursive": true,
            "preload": false,
            "prerender": true
          },
          "path": "/orders/:id",
          "id": "_orders__id",
          "component": () => import('../src/pages/orders/[id].svelte').then(m => m.default)
        },
        {
          "isFile": true,
          "isDir": false,
          "file": "create.svelte",
          "filepath": "/orders/create.svelte",
          "name": "create",
          "ext": "svelte",
          "badExt": false,
          "absolutePath": "D:/Projects/Sheaft/api/Sheaft.Web.Portal/src/pages/orders/create.svelte",
          "importPath": "../src/pages/orders/create.svelte",
          "isLayout": false,
          "isReset": false,
          "isIndex": false,
          "isFallback": false,
          "isPage": true,
          "ownMeta": {
            "menu": "Créer",
            "index": 2,
            "title": "Créer une commande"
          },
          "meta": {
            "menu": "Créer",
            "index": 2,
            "title": "Créer une commande",
            "recursive": true,
            "preload": false,
            "prerender": true
          },
          "path": "/orders/create",
          "id": "_orders_create",
          "component": () => import('../src/pages/orders/create.svelte').then(m => m.default)
        }
      ],
      "isLayout": true,
      "isReset": false,
      "isIndex": false,
      "isFallback": false,
      "isPage": false,
      "importPath": "../src/pages/orders/_layout.svelte",
      "ownMeta": {
        "menu": "Commandes",
        "index": 4,
        "group": true,
        "icon": "fas#fileLines"
      },
      "meta": {
        "menu": "Commandes",
        "index": 4,
        "group": true,
        "icon": "fas#fileLines",
        "recursive": true,
        "preload": false,
        "prerender": true
      },
      "path": "/orders",
      "id": "_orders__layout",
      "component": () => import('../src/pages/orders/_layout.svelte').then(m => m.default)
    },
    {
      "isFile": true,
      "isDir": true,
      "file": "_layout.svelte",
      "filepath": "/deliveries/_layout.svelte",
      "name": "_layout",
      "ext": "svelte",
      "badExt": false,
      "absolutePath": "D:/Projects/Sheaft/api/Sheaft.Web.Portal/src/pages/deliveries/_layout.svelte",
      "children": [
        {
          "isFile": true,
          "isDir": false,
          "file": "[id].svelte",
          "filepath": "/deliveries/[id].svelte",
          "name": "[id]",
          "ext": "svelte",
          "badExt": false,
          "absolutePath": "D:/Projects/Sheaft/api/Sheaft.Web.Portal/src/pages/deliveries/[id].svelte",
          "importPath": "../src/pages/deliveries/[id].svelte",
          "isLayout": false,
          "isReset": false,
          "isIndex": false,
          "isFallback": false,
          "isPage": true,
          "ownMeta": {
            "index": true,
            "title": "Details de la livraison",
            "roles": []
          },
          "meta": {
            "index": true,
            "title": "Details de la livraison",
            "roles": [],
            "recursive": true,
            "preload": false,
            "prerender": true
          },
          "path": "/deliveries/:id",
          "id": "_deliveries__id",
          "component": () => import('../src/pages/deliveries/[id].svelte').then(m => m.default)
        },
        {
          "isFile": true,
          "isDir": false,
          "file": "index.svelte",
          "filepath": "/deliveries/index.svelte",
          "name": "index",
          "ext": "svelte",
          "badExt": false,
          "absolutePath": "D:/Projects/Sheaft/api/Sheaft.Web.Portal/src/pages/deliveries/index.svelte",
          "importPath": "../src/pages/deliveries/index.svelte",
          "isLayout": false,
          "isReset": false,
          "isIndex": true,
          "isFallback": false,
          "isPage": true,
          "ownMeta": {
            "menu": "Mes livraisons",
            "title": "Livraisons",
            "roles": [],
            "index": 1,
            "default": true
          },
          "meta": {
            "menu": "Mes livraisons",
            "title": "Livraisons",
            "roles": [],
            "index": 1,
            "default": true,
            "recursive": true,
            "preload": false,
            "prerender": true
          },
          "path": "/deliveries/index",
          "id": "_deliveries_index",
          "component": () => import('../src/pages/deliveries/index.svelte').then(m => m.default)
        },
        {
          "isFile": true,
          "isDir": false,
          "file": "create.svelte",
          "filepath": "/deliveries/create.svelte",
          "name": "create",
          "ext": "svelte",
          "badExt": false,
          "absolutePath": "D:/Projects/Sheaft/api/Sheaft.Web.Portal/src/pages/deliveries/create.svelte",
          "importPath": "../src/pages/deliveries/create.svelte",
          "isLayout": false,
          "isReset": false,
          "isIndex": false,
          "isFallback": false,
          "isPage": true,
          "ownMeta": {
            "menu": "Créer",
            "index": 2,
            "title": "Ajouter un nouveau produit",
            "icon": "fas#coffee"
          },
          "meta": {
            "menu": "Créer",
            "index": 2,
            "title": "Ajouter un nouveau produit",
            "icon": "fas#coffee",
            "recursive": true,
            "preload": false,
            "prerender": true
          },
          "path": "/deliveries/create",
          "id": "_deliveries_create",
          "component": () => import('../src/pages/deliveries/create.svelte').then(m => m.default)
        }
      ],
      "isLayout": true,
      "isReset": false,
      "isIndex": false,
      "isFallback": false,
      "isPage": false,
      "importPath": "../src/pages/deliveries/_layout.svelte",
      "ownMeta": {
        "menu": "Livraisons",
        "index": 5,
        "group": true,
        "icon": "fas#truckRampBox"
      },
      "meta": {
        "menu": "Livraisons",
        "index": 5,
        "group": true,
        "icon": "fas#truckRampBox",
        "recursive": true,
        "preload": false,
        "prerender": true
      },
      "path": "/deliveries",
      "id": "_deliveries__layout",
      "component": () => import('../src/pages/deliveries/_layout.svelte').then(m => m.default)
    },
    {
      "isFile": true,
      "isDir": true,
      "file": "_layout.svelte",
      "filepath": "/billings/_layout.svelte",
      "name": "_layout",
      "ext": "svelte",
      "badExt": false,
      "absolutePath": "D:/Projects/Sheaft/api/Sheaft.Web.Portal/src/pages/billings/_layout.svelte",
      "children": [
        {
          "isFile": true,
          "isDir": false,
          "file": "[id].svelte",
          "filepath": "/billings/[id].svelte",
          "name": "[id]",
          "ext": "svelte",
          "badExt": false,
          "absolutePath": "D:/Projects/Sheaft/api/Sheaft.Web.Portal/src/pages/billings/[id].svelte",
          "importPath": "../src/pages/billings/[id].svelte",
          "isLayout": false,
          "isReset": false,
          "isIndex": false,
          "isFallback": false,
          "isPage": true,
          "ownMeta": {
            "index": true,
            "title": "Details de la facture",
            "roles": []
          },
          "meta": {
            "index": true,
            "title": "Details de la facture",
            "roles": [],
            "recursive": true,
            "preload": false,
            "prerender": true
          },
          "path": "/billings/:id",
          "id": "_billings__id",
          "component": () => import('../src/pages/billings/[id].svelte').then(m => m.default)
        },
        {
          "isFile": true,
          "isDir": false,
          "file": "index.svelte",
          "filepath": "/billings/index.svelte",
          "name": "index",
          "ext": "svelte",
          "badExt": false,
          "absolutePath": "D:/Projects/Sheaft/api/Sheaft.Web.Portal/src/pages/billings/index.svelte",
          "importPath": "../src/pages/billings/index.svelte",
          "isLayout": false,
          "isReset": false,
          "isIndex": true,
          "isFallback": false,
          "isPage": true,
          "ownMeta": {
            "menu": "Mes factures",
            "title": "Factures",
            "roles": [],
            "index": 1,
            "default": true
          },
          "meta": {
            "menu": "Mes factures",
            "title": "Factures",
            "roles": [],
            "index": 1,
            "default": true,
            "recursive": true,
            "preload": false,
            "prerender": true
          },
          "path": "/billings/index",
          "id": "_billings_index",
          "component": () => import('../src/pages/billings/index.svelte').then(m => m.default)
        },
        {
          "isFile": true,
          "isDir": false,
          "file": "create.svelte",
          "filepath": "/billings/create.svelte",
          "name": "create",
          "ext": "svelte",
          "badExt": false,
          "absolutePath": "D:/Projects/Sheaft/api/Sheaft.Web.Portal/src/pages/billings/create.svelte",
          "importPath": "../src/pages/billings/create.svelte",
          "isLayout": false,
          "isReset": false,
          "isIndex": false,
          "isFallback": false,
          "isPage": true,
          "ownMeta": {
            "menu": "Créer",
            "index": 2,
            "title": "Ajouter un nouveau produit",
            "icon": "fas#coffee"
          },
          "meta": {
            "menu": "Créer",
            "index": 2,
            "title": "Ajouter un nouveau produit",
            "icon": "fas#coffee",
            "recursive": true,
            "preload": false,
            "prerender": true
          },
          "path": "/billings/create",
          "id": "_billings_create",
          "component": () => import('../src/pages/billings/create.svelte').then(m => m.default)
        }
      ],
      "isLayout": true,
      "isReset": false,
      "isIndex": false,
      "isFallback": false,
      "isPage": false,
      "importPath": "../src/pages/billings/_layout.svelte",
      "ownMeta": {
        "menu": "Facturation",
        "index": 6,
        "group": true,
        "icon": "fas#fileInvoiceDollar"
      },
      "meta": {
        "menu": "Facturation",
        "index": 6,
        "group": true,
        "icon": "fas#fileInvoiceDollar",
        "recursive": true,
        "preload": false,
        "prerender": true
      },
      "path": "/billings",
      "id": "_billings__layout",
      "component": () => import('../src/pages/billings/_layout.svelte').then(m => m.default)
    },
    {
      "isFile": true,
      "isDir": true,
      "file": "_layout.svelte",
      "filepath": "/documents/_layout.svelte",
      "name": "_layout",
      "ext": "svelte",
      "badExt": false,
      "absolutePath": "D:/Projects/Sheaft/api/Sheaft.Web.Portal/src/pages/documents/_layout.svelte",
      "children": [
        {
          "isFile": true,
          "isDir": false,
          "file": "index.svelte",
          "filepath": "/documents/index.svelte",
          "name": "index",
          "ext": "svelte",
          "badExt": false,
          "absolutePath": "D:/Projects/Sheaft/api/Sheaft.Web.Portal/src/pages/documents/index.svelte",
          "importPath": "../src/pages/documents/index.svelte",
          "isLayout": false,
          "isReset": false,
          "isIndex": true,
          "isFallback": false,
          "isPage": true,
          "ownMeta": {
            "menu": "Mes documents",
            "title": "Documents",
            "index": true,
            "roles": [],
            "icon": "fas#book",
            "default": true
          },
          "meta": {
            "menu": "Mes documents",
            "title": "Documents",
            "index": true,
            "roles": [],
            "icon": "fas#book",
            "default": true,
            "recursive": true,
            "preload": false,
            "prerender": true
          },
          "path": "/documents/index",
          "id": "_documents_index",
          "component": () => import('../src/pages/documents/index.svelte').then(m => m.default)
        }
      ],
      "isLayout": true,
      "isReset": false,
      "isIndex": false,
      "isFallback": false,
      "isPage": false,
      "importPath": "../src/pages/documents/_layout.svelte",
      "ownMeta": {
        "menu": "Documents",
        "group": true,
        "index": 7,
        "icon": "fas#book"
      },
      "meta": {
        "menu": "Documents",
        "group": true,
        "index": 7,
        "icon": "fas#book",
        "recursive": true,
        "preload": false,
        "prerender": true
      },
      "path": "/documents",
      "id": "_documents__layout",
      "component": () => import('../src/pages/documents/_layout.svelte').then(m => m.default)
    }
  ],
  "isLayout": true,
  "isReset": false,
  "isIndex": false,
  "isFallback": false,
  "isPage": false,
  "isFile": true,
  "file": "_layout.svelte",
  "ext": "svelte",
  "badExt": false,
  "importPath": "../src/pages/_layout.svelte",
  "meta": {
    "recursive": true,
    "preload": false,
    "prerender": true
  },
  "path": "/",
  "id": "__layout",
  "component": () => import('../src/pages/_layout.svelte').then(m => m.default)
}


export const {tree, routes} = buildClientTree(_tree)

