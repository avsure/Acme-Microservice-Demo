# Stage 1: Build Angular App
FROM node:22 AS build
WORKDIR /app

# Copy package files first (better cache)
COPY package*.json ./
RUN npm install

# Copy rest of the app
COPY . .

# Build Angular app
RUN npm run build --prod

# Stage 2: Serve using NGINX
FROM nginx:stable

# Remove default NGINX site
RUN rm -rf /usr/share/nginx/html/*

# Copy build output to NGINX
COPY --from=build /app/dist/acme-frontend-angular/browser /usr/share/nginx/html

# Expose port
EXPOSE 80

# Start NGINX
CMD ["nginx", "-g", "daemon off;"]
