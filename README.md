# elasticsearchkata
Repositorio para la formación de ElasticSearch

	1: Instalar las últimas versiones de Docker + Chrome + PostMan.		
	2: Ejecutar en docker (abrir powershell y pegar): docker run -p 9200:9200 -p 9300:9300 -e "discovery.type=single-node" --name elastic_curso docker.elastic.co/elasticsearch/elasticsearch:6.2.4
	3: Comprobar que se este ejecutando el elastic en: http://localhost:9200/
	4: Instalar la extensión: https://chrome.google.com/webstore/detail/elasticsearch-head/ffmkiejjmecolpfloofpjologoblkegm/related
	5: Ejecutar la extensión (icono arriba a la derecha del chrome) y conectarse a: http://localhost:9200/
	6: Importar la colección de consultas al PostMan, estan en la carpeta \PostMan del proyecto. O también la podeis importar de aqui: https://www.getpostman.com/collections/a6ae4fd5619642dc6b91
	7: Obtener la última versión del proyecto, compilar e ejecutarlo.
