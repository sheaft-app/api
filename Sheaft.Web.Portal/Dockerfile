FROM node:12 AS build

WORKDIR /appnpm 

COPY package.json ./
COPY package-lock.json ./
RUN npm install
COPY . ./
RUN npm run build

FROM nginx:1.19-alpine
COPY --from=build /app/wwwroot /usr/share/nginx/html
