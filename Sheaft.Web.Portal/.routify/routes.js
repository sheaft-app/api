
/**
 * @roxi/routify 2.18.5
 * File generated Fri May 13 2022 18:58:09 GMT+0200 (heure d’été d’Europe centrale)
 */

export const __version = "2.18.5"
export const __timestamp = "2022-05-13T16:58:09.560Z"

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
            "public": true,
            "redirectIfAuthenticated": true
          },
          "meta": {
            "public": true,
            "redirectIfAuthenticated": true,
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
            "public": true,
            "redirectIfAuthenticated": true
          },
          "meta": {
            "public": true,
            "redirectIfAuthenticated": true,
            "recursive": true,
            "preload": false,
            "prerender": true
          },
          "path": "/auth/login",
          "id": "_auth_login",
          "component": () => import('../src/pages/auth/login.svelte').then(m => m.default)
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
      "isDir": true,
      "file": "_layout.svelte",
      "filepath": "/sub/_layout.svelte",
      "name": "_layout",
      "ext": "svelte",
      "badExt": false,
      "absolutePath": "D:/Projects/Sheaft/api/Sheaft.Web.Portal/src/pages/sub/_layout.svelte",
      "children": [
        {
          "isFile": true,
          "isDir": false,
          "file": "create.svelte",
          "filepath": "/sub/create.svelte",
          "name": "create",
          "ext": "svelte",
          "badExt": false,
          "absolutePath": "D:/Projects/Sheaft/api/Sheaft.Web.Portal/src/pages/sub/create.svelte",
          "importPath": "../src/pages/sub/create.svelte",
          "isLayout": false,
          "isReset": false,
          "isIndex": false,
          "isFallback": false,
          "isPage": true,
          "ownMeta": {
            "public": true,
            "menu": true,
            "title": "Create",
            "roles": [
              "User",
              "Admin"
            ]
          },
          "meta": {
            "public": true,
            "menu": true,
            "title": "Create",
            "roles": [
              "User",
              "Admin"
            ],
            "recursive": true,
            "preload": false,
            "prerender": true
          },
          "path": "/sub/create",
          "id": "_sub_create",
          "component": () => import('../src/pages/sub/create.svelte').then(m => m.default)
        },
        {
          "isFile": true,
          "isDir": false,
          "file": "[id].svelte",
          "filepath": "/sub/[id].svelte",
          "name": "[id]",
          "ext": "svelte",
          "badExt": false,
          "absolutePath": "D:/Projects/Sheaft/api/Sheaft.Web.Portal/src/pages/sub/[id].svelte",
          "importPath": "../src/pages/sub/[id].svelte",
          "isLayout": false,
          "isReset": false,
          "isIndex": false,
          "isFallback": false,
          "isPage": true,
          "ownMeta": {
            "public": true,
            "index": true,
            "title": "Details",
            "roles": [
              "User",
              "Admin"
            ]
          },
          "meta": {
            "public": true,
            "index": true,
            "title": "Details",
            "roles": [
              "User",
              "Admin"
            ],
            "recursive": true,
            "preload": false,
            "prerender": true
          },
          "path": "/sub/:id",
          "id": "_sub__id",
          "component": () => import('../src/pages/sub/[id].svelte').then(m => m.default)
        },
        {
          "isFile": true,
          "isDir": false,
          "file": "index.svelte",
          "filepath": "/sub/index.svelte",
          "name": "index",
          "ext": "svelte",
          "badExt": false,
          "absolutePath": "D:/Projects/Sheaft/api/Sheaft.Web.Portal/src/pages/sub/index.svelte",
          "importPath": "../src/pages/sub/index.svelte",
          "isLayout": false,
          "isReset": false,
          "isIndex": true,
          "isFallback": false,
          "isPage": true,
          "ownMeta": {
            "public": true,
            "menu": true,
            "title": "Index",
            "roles": [
              "User",
              "Admin"
            ],
            "index": true
          },
          "meta": {
            "public": true,
            "menu": true,
            "title": "Index",
            "roles": [
              "User",
              "Admin"
            ],
            "index": true,
            "recursive": true,
            "preload": false,
            "prerender": true
          },
          "path": "/sub/index",
          "id": "_sub_index",
          "component": () => import('../src/pages/sub/index.svelte').then(m => m.default)
        }
      ],
      "isLayout": true,
      "isReset": false,
      "isIndex": false,
      "isFallback": false,
      "isPage": false,
      "importPath": "../src/pages/sub/_layout.svelte",
      "ownMeta": {
        "group": "Commandes",
        "index": true
      },
      "meta": {
        "group": "Commandes",
        "index": true,
        "recursive": true,
        "preload": false,
        "prerender": true
      },
      "path": "/sub",
      "id": "_sub__layout",
      "component": () => import('../src/pages/sub/_layout.svelte').then(m => m.default)
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
        "menu": true,
        "title": "Contact",
        "index": true
      },
      "meta": {
        "public": true,
        "menu": true,
        "title": "Contact",
        "index": true,
        "recursive": true,
        "preload": false,
        "prerender": true
      },
      "path": "/contact",
      "id": "_contact",
      "component": () => import('../src/pages/contact.svelte').then(m => m.default)
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
        "menu": true,
        "title": "Home",
        "index": true
      },
      "meta": {
        "menu": true,
        "title": "Home",
        "index": true,
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
      "isDir": false,
      "file": "test.svelte",
      "filepath": "/test.svelte",
      "name": "test",
      "ext": "svelte",
      "badExt": false,
      "absolutePath": "D:/Projects/Sheaft/api/Sheaft.Web.Portal/src/pages/test.svelte",
      "importPath": "../src/pages/test.svelte",
      "isLayout": false,
      "isReset": false,
      "isIndex": false,
      "isFallback": false,
      "isPage": true,
      "ownMeta": {
        "title": "Test",
        "public": true,
        "menu": true,
        "index": true
      },
      "meta": {
        "title": "Test",
        "public": true,
        "menu": true,
        "index": true,
        "recursive": true,
        "preload": false,
        "prerender": true
      },
      "path": "/test",
      "id": "_test",
      "component": () => import('../src/pages/test.svelte').then(m => m.default)
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

