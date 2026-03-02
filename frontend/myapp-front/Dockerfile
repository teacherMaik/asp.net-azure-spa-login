FROM node:20 AS build
WORKDIR /app
COPY package*.json ./
RUN npm install
COPY . .
ARG ANGULAR_ENV=production
RUN if [ "$ANGULAR_ENV" = "production" ]; then \
      npm run build -- --configuration production; \
    else \
      npm run build -- --configuration development; \
    fi

# --- DEBUG STEP ---
# This will print the EXACT path of every file in the dist folder
RUN find /app/dist -name index.html
# ------------------

# Serve with ngix
FROM nginx:alpine
COPY --from=build /app/dist/myapp-front/browser/ /usr/share/nginx/html

COPY nginx.conf /etc/nginx/conf.d/default.conf

EXPOSE 80