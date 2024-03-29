
BACKEND_FILES := Backend/source/Dockerfile $(wildcard Backend/source/*.cs) $(wildcard Backend/source/**/*.cs)
FRONTEND_FILES := Frontend/Dockerfile Frontend/vite.config.ts $(wildcard Frontend/src/*.tsx) \
									$(wildcard Frontend/src/*.css) $(wildcard Frontend/src/*.ts) \
									$(wildcard Frontend/src/**/*.tsx) $(wildcard Frontend/src/**/*.ts)

include .env

app: docker-compose.yml frontend backend
	docker-compose up -d

frontend: $(FRONTEND_FILES)
	docker build -t signalr-omagol_viteapp ./Frontend --build-arg VITE_BACKEND=$(VITE_BACKEND)
	touch frontend

backend: $(BACKEND_FILES)
	docker build -t signalr-omagol_backend ./Backend/source
	touch backend

.PHONY: stop clean

stop:
	docker-compose down

clean:
	rm -f backend frontend
