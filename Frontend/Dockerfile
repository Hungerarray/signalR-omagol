# build environment
FROM node:17-alpine AS build-env
WORKDIR /app

# Copy over files and install modules
COPY ./package*.json .
RUN npm install

ARG VITE_BACKEND
ENV VITE_BACKEND ${VITE_BACKEND}

COPY . . 
RUN npx vite build

# Run environment
FROM nginx:1.21-alpine
COPY --from=build-env /app/dist /usr/share/nginx/html

