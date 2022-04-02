
BACKEND_FILES := Backend/Dockerfile $(wildcard Backend/*.cs) $(wildcard Backend/**/*.cs)
FRONTEND_FILES := Frontend/Dockerfile Frontend/vite.config.ts $(wildcard Frontend/src/*.tsx) \
									$(wildcard Frontend/src/*.css) $(wildcard Frontend/src/*.ts) \
									$(wildcard Frontend/src/**/*.tsx) $(wildcard Frontend/src/**/*.ts)

app: docker-compose.yml frontend backend
	docker-compose up -d

frontend: $(FRONTEND_FILES)
	docker build -t signalr-omagol_viteapp ./Frontend
	touch frontend

backend: $(BACKEND_FILES)
	docker build -t signalr-omagol_backend ./Backend
	touch backend

.PHONY: stop

stop:
	docker-compose down
