# build environment
FROM node:17 AS build-env
WORKDIR /app

# Copy over files and install modules
COPY ./package*.json .
RUN npm install

COPY . . 
RUN npx vite build

# Run environment
FROM nginx:1.21.6
COPY --from=build-env /app/dist /usr/share/nginx/html

