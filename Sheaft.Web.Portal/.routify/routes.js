
/**
 * @roxi/routify 2.18.5
 * File generated Mon May 23 2022 19:09:39 GMT+0200 (heure d’été d’Europe centrale)
 */

export const __version = "2.18.5"
export const __timestamp = "2022-05-23T17:09:39.581Z"

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
        "icon": "fas#house"
      },
      "meta": {
        "public": true,
        "index": 1,
        "menu": "Accueil",
        "title": "Bienvenue sur votre dashboard",
        "icon": "fas#house",
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
      "filepath": "/orders/_layout.svelte",
      "name": "_layout",
      "ext": "svelte",
      "badExt": false,
      "absolutePath": "D:/Projects/Sheaft/api/Sheaft.Web.Portal/src/pages/orders/_layout.svelte",
      "children": [
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
            "title": "Details",
            "roles": []
          },
          "meta": {
            "index": true,
            "title": "Details",
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
            "menu": "Vos commandes",
            "title": "Vos commandes en cours",
            "roles": [],
            "index": 1,
            "icon": "fas#addressBook",
            "default": true
          },
          "meta": {
            "menu": "Vos commandes",
            "title": "Vos commandes en cours",
            "roles": [],
            "index": 1,
            "icon": "fas#addressBook",
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
            "title": "Créer une commande",
            "icon": "fas#coffee"
          },
          "meta": {
            "menu": "Créer",
            "index": 2,
            "title": "Créer une commande",
            "icon": "fas#coffee",
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
        "index": 2,
        "group": true,
        "icon": "fas#coffee"
      },
      "meta": {
        "menu": "Commandes",
        "index": 2,
        "group": true,
        "icon": "fas#coffee",
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
      "isDir": false,
      "file": "contact.svelte",
      "filepath": "/contact.svelte",
      "name": "contact",
      "ext": "svelte",
      "badExt": false,
      "absolutePath": "D:/Projects/Sheaft/api/Sheaft.Web.Portal/src/pages/contact.svelte",
      "importPath": "../src/pages/contact.svelte",
      "isLayout": false,
      "isReset": false,
      "isIndex": false,
      "isFallback": false,
      "isPage": true,
      "ownMeta": {
        "public": true,
        "menu": "Contact",
        "title": "Contactez-nous",
        "index": 3,
        "icon": "fas#addressBook"
      },
      "meta": {
        "public": true,
        "menu": "Contact",
        "title": "Contactez-nous",
        "index": 3,
        "icon": "fas#addressBook",
        "recursive": true,
        "preload": false,
        "prerender": true
      },
      "path": "/contact",
      "id": "_contact",
      "component": () => import('../src/pages/contact.svelte').then(m => m.default)
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

