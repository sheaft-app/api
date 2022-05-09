const G=function(){const t=document.createElement("link").relList;if(t&&t.supports&&t.supports("modulepreload"))return;for(const r of document.querySelectorAll('link[rel="modulepreload"]'))o(r);new MutationObserver(r=>{for(const s of r)if(s.type==="childList")for(const i of s.addedNodes)i.tagName==="LINK"&&i.rel==="modulepreload"&&o(i)}).observe(document,{childList:!0,subtree:!0});function n(r){const s={};return r.integrity&&(s.integrity=r.integrity),r.referrerpolicy&&(s.referrerPolicy=r.referrerpolicy),r.crossorigin==="use-credentials"?s.credentials="include":r.crossorigin==="anonymous"?s.credentials="omit":s.credentials="same-origin",s}function o(r){if(r.ep)return;r.ep=!0;const s=n(r);fetch(r.href,s)}};G();function $(){}function H(e){return e()}function T(){return Object.create(null)}function L(e){e.forEach(H)}function J(e){return typeof e=="function"}function P(e,t){return e!=e?t==t:e!==t||e&&typeof e=="object"||typeof e=="function"}let b;function Q(e,t){return b||(b=document.createElement("a")),b.href=t,e===b.href}function R(e){return Object.keys(e).length===0}function d(e,t){e.appendChild(t)}function B(e,t,n){e.insertBefore(t,n||null)}function N(e){e.parentNode.removeChild(e)}function h(e){return document.createElement(e)}function C(e){return document.createTextNode(e)}function x(){return C(" ")}function U(e,t,n,o){return e.addEventListener(t,n,o),()=>e.removeEventListener(t,n,o)}function p(e,t,n){n==null?e.removeAttribute(t):e.getAttribute(t)!==n&&e.setAttribute(t,n)}function W(e){return Array.from(e.childNodes)}function X(e,t){t=""+t,e.wholeText!==t&&(e.data=t)}let j;function y(e){j=e}const g=[],M=[],E=[],q=[],Y=Promise.resolve();let O=!1;function Z(){O||(O=!0,Y.then(I))}function S(e){E.push(e)}const A=new Set;let w=0;function I(){const e=j;do{for(;w<g.length;){const t=g[w];w++,y(t),ee(t.$$)}for(y(null),g.length=0,w=0;M.length;)M.pop()();for(let t=0;t<E.length;t+=1){const n=E[t];A.has(n)||(A.add(n),n())}E.length=0}while(g.length);for(;q.length;)q.pop()();O=!1,A.clear(),y(e)}function ee(e){if(e.fragment!==null){e.update(),L(e.before_update);const t=e.dirty;e.dirty=[-1],e.fragment&&e.fragment.p(e.ctx,t),e.after_update.forEach(S)}}const k=new Set;let te;function K(e,t){e&&e.i&&(k.delete(e),e.i(t))}function ne(e,t,n,o){if(e&&e.o){if(k.has(e))return;k.add(e),te.c.push(()=>{k.delete(e),o&&(n&&e.d(1),o())}),e.o(t)}}function re(e){e&&e.c()}function V(e,t,n,o){const{fragment:r,on_mount:s,on_destroy:i,after_update:c}=e.$$;r&&r.m(t,n),o||S(()=>{const f=s.map(H).filter(J);i?i.push(...f):L(f),e.$$.on_mount=[]}),c.forEach(S)}function F(e,t){const n=e.$$;n.fragment!==null&&(L(n.on_destroy),n.fragment&&n.fragment.d(t),n.on_destroy=n.fragment=null,n.ctx=[])}function oe(e,t){e.$$.dirty[0]===-1&&(g.push(e),Z(),e.$$.dirty.fill(0)),e.$$.dirty[t/31|0]|=1<<t%31}function z(e,t,n,o,r,s,i,c=[-1]){const f=j;y(e);const l=e.$$={fragment:null,ctx:null,props:s,update:$,not_equal:r,bound:T(),on_mount:[],on_destroy:[],on_disconnect:[],before_update:[],after_update:[],context:new Map(t.context||(f?f.$$.context:[])),callbacks:T(),dirty:c,skip_bound:!1,root:t.target||f.$$.root};i&&i(l.root);let _=!1;if(l.ctx=n?n(e,t.props||{},(u,m,...a)=>{const v=a.length?a[0]:m;return l.ctx&&r(l.ctx[u],l.ctx[u]=v)&&(!l.skip_bound&&l.bound[u]&&l.bound[u](v),_&&oe(e,u)),m}):[],l.update(),_=!0,L(l.before_update),l.fragment=o?o(l.ctx):!1,t.target){if(t.hydrate){const u=W(t.target);l.fragment&&l.fragment.l(u),u.forEach(N)}else l.fragment&&l.fragment.c();t.intro&&K(e.$$.fragment),V(e,t.target,t.anchor,t.customElement),I()}y(f)}class D{$destroy(){F(this,1),this.$destroy=$}$on(t,n){const o=this.$$.callbacks[t]||(this.$$.callbacks[t]=[]);return o.push(n),()=>{const r=o.indexOf(n);r!==-1&&o.splice(r,1)}}$set(t){this.$$set&&!R(t)&&(this.$$.skip_bound=!0,this.$$set(t),this.$$.skip_bound=!1)}}var se="/assets/svelte.d72399d3.png";function le(e){let t,n,o,r,s;return{c(){t=h("button"),n=C("Clicks: "),o=C(e[0]),p(t,"class","btn-primary hover:bg-gray-300")},m(i,c){B(i,t,c),d(t,n),d(t,o),r||(s=U(t,"click",e[1]),r=!0)},p(i,[c]){c&1&&X(o,i[0])},i:$,o:$,d(i){i&&N(t),r=!1,s()}}}function ie(e,t,n){let o=0;return[o,()=>{n(0,o+=1)}]}class ce extends D{constructor(t){super(),z(this,t,ie,le,P,{})}}function ue(e){let t,n,o,r,s,i,c,f,l,_,u,m;return c=new ce({}),{c(){t=h("main"),n=h("img"),r=x(),s=h("h1"),s.textContent="Hello Typescript!",i=x(),re(c.$$.fragment),f=x(),l=h("p"),l.innerHTML='Visit <a href="https://svelte.dev">svelte.dev</a> to learn how to build Svelte apps.',_=x(),u=h("p"),u.innerHTML=`Check out <a href="https://github.com/sveltejs/kit#readme">SvelteKit</a> for the officially
    supported framework, also powered by Vite!`,Q(n.src,o=se)||p(n,"src",o),p(n,"alt","Svelte Logo"),p(n,"class","svelte-164ron2"),p(s,"class","svelte-164ron2"),p(l,"class","svelte-164ron2"),p(u,"class","svelte-164ron2"),p(t,"class","svelte-164ron2")},m(a,v){B(a,t,v),d(t,n),d(t,r),d(t,s),d(t,i),V(c,t,null),d(t,f),d(t,l),d(t,_),d(t,u),m=!0},p:$,i(a){m||(K(c.$$.fragment,a),m=!0)},o(a){ne(c.$$.fragment,a),m=!1},d(a){a&&N(t),F(c)}}}class fe extends D{constructor(t){super(),z(this,t,null,ue,P,{})}}new fe({target:document.getElementById("app")});
