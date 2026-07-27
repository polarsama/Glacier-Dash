# GAME DESIGN DOCUMENT (GDD)

## 1. INFORMACION GENERAL
* **Nombre del Proyecto (Provisional):** Glacier Dash
* **Tematica / Premisa Central:** A darle caña (Velocidad, esfuerzo y aceleracion extrema)
* **Genero:** Arcade / Runner Infinito de Velocidad en 2D
* **Plataforma:** PC / Web (WebGL)
* **Motor de Desarrollo:** Unity (2D)
* **Publico Objetivo:** Jugadores de Game Jams y amantes de juegos arcade de partidas rapidas
* **Duracion del Desarrollo:** 1 Semana (Game Jam)
* **Desarrollador:** Solo Dev (1 Persona)

---

## 2. CONCEPTO Y VISION

### 2.1. Resumen Ejecutivo
Glacier Dash es un juego de accion arcade en 2D inspirado en la estructura de navegacion y flujo continuo de juegos de carrera infinita pero enfocado en la velocidad y la destruccion de obstaculos. El jugador controla a un oso polar que se desliza a toda velocidad montaña abajo huyendo de una avalancha masiva. El objetivo es mantener el impulso, destruir bloques de hielo y acumular energia para activar el estado de invencibilidad.

### 2.2. Pilares de Diseño
1. **Velocidad Impensada:** El ritmo del juego aumenta progresivamente y premia al jugador que arriesga para ir mas rapido.
2. **Controles Directos:** Un solo boton para ejecutar todas las acciones principales, permitiendo un aprendizaje inmediato.
3. **Retroalimentacion Visual (Juiciness):** Uso intensivo de temblor de camara, sistemas de particulas y efectos de rastro para transmitir impacto y aceleracion.

---

## 3. MECANICAS DE JUEGO (GAMEPLAY)

### 3.1. Controles
* **Espacio / Clic Izquierdo / Boton Primario (Input Fire1):** Ejecutar Turbo-Dash (Aceleracion lineal e impacto hacia adelante).

### 3.2. Bucle Principal de Juego (Core Loop)
1. El personaje se desliza automaticamente montaña abajo ganando velocidad progresiva.
2. El jugador presiona el boton de entrada para ejecutar un Turbo-Dash en el momento adecuado.
3. El Turbo-Dash permite atravesar obstaculos de hielo, ganar distancia y recargar la Barra de Caña.
4. Si el jugador comete errores, pierde velocidad y corre el riesgo de ser atrapado por la avalancha o caer en un precipicio.
5. Al llenar la barra al 100%, se activa el Modo Caña (invencibilidad temporal).

### 3.3. Sistema de Obstaculos e Impactos
A diferencia de los juegos donde cualquier contacto genera una muerte instantanea, este titulo utiliza un sistema basado en la perdida de impulso:
* **Bloques de Hielo Menores:** Se destruyen al impacto pero reducen la velocidad del jugador en un 15% si no se realiza un Turbo-Dash.
* **Rocas y Obstaculos Pesados:** Reducen la velocidad del jugador en un 35% y detienen la racha de combos si se impactan en estado normal.
* **Precipicios y Grietas:** Huecos en el terreno que requieren calcular la velocidad para ser cruzados.

### 3.4. Condiciones de Derrota (Game Over)
Existen dos formas unicas de perder la partida:
1. **Captura por Avalancha:** Una avalancha avanza constantemente desde la parte izquierda de la pantalla. Si el jugador choca repetidamente contra obstaculos y pierde velocidad, la avalancha lo alcanza y finaliza la partida.
2. **Caida al Vacio:** Si el jugador no calcula la trayectoria o la velocidad antes de una grieta en el terreno y cae al abismo, se produce un Game Over instantaneo.

### 3.5. Mecanica Especial: Modo Caña (Glacier Heat)
* **Acumulacion:** Cada Turbo-Dash ejecutado correctamente suma un 5% a la barra. Cada obstaculo destruido mediante el Turbo-Dash suma un 10%.
* **Activacion:** Al alcanzar el 100%, el personaje entra en estado frenetico durante 5 segundos.
* **Efectos Durante el Estado:**
  * Invencibilidad total frente a rocas y obstaculos pesados.
  * Multiplicador de puntuacion x2.
  * Incremento automatico de la velocidad de desplazamiento al doble de la velocidad base.
  * Cambio estético en la paleta de colores y aumento del efecto de temblor de camara.

---

## 4. ESTRUCTURA TECNICA Y ARQUITECTURA EN UNITY

### 4.1. Configuración de Escenas
* **Escena 0 (MainMenu):** Interfaz basica con boton de inicio, control de volumen y puntuacion maxima registrada.
* **Escena 1 (GameScene):** Bucle principal de juego con el nivel infinito, interfaz en tiempo real y panel de Game Over.

### 4.2. Scripts Principales

#### PlayerController.cs
* Controla la velocidad de desplazamiento del Rigidbody2D.
* Procesa la entrada del usuario (`Input.GetButtonDown("Fire1")`).
* Aplica la fuerza del Turbo-Dash y gestiona la duracion del estado de aceleracion.
* Lleva el conteo de la barra de energia para la activacion del Modo Caña.

#### CameraFollow.cs
* Mantiene la camara alineada con la posicion X del jugador manteniendo un desfase (offset) constante.
* Incluye la funcion de `CameraShake` invocada mediante eventos o llamadas directas tras los impactos.

#### ObjectSpawner.cs / LevelGenerator.cs
* Implementa un sistema de reutilizacion de objetos (Object Pooling) para instanciar tramos de terreno, obstaculos y precipicios por delante de la camara.
* Desactiva y reposiciona los elementos que quedan por detras del margen izquierdo de la camara para optimizar el rendimiento.

#### AvalancheManager.cs
* Mantiene un objeto en movimiento constante a una velocidad calibrada justo detras del jugador.
* Ajusta su distancia en funcion de los frenazos que sufre el jugador al chocar con obstaculos.

#### GameManager.cs
* Administra el estado global de la partida (Inicio, En Juego, Pausa, Game Over).
* Calcula la puntuacion basada en la distancia recorrida y multiplicadores activos.
* Gestiona el reinicio rapido de la escena mediante `SceneManager.LoadScene()`.

---

## 5. DISEÑO ARTISTICO Y SONORO

### 5.1. Arte 2D
* **Estilo:** Pixel Art / Sprites 2D simplificados.
* **Protagonista:** Oso polar en posicion de deslizamiento con animaciones basicas para estado normal, Turbo-Dash y derrota.
* **Entorno:** Laderas nevadas, bloques de hielo translúcidos, rocas oscuras y un fondo con efecto Parallax de montañas lejanas.
* **Efectos Visuales:**
  * Componente `Trail Renderer` adjunto al jugador para generar una estela continua de color cyan / azul neón.
  * Componente `Particle System` para emitir esquirlas de hielo al destruir objetos.

### 5.2. Audio
* **Música:** Una pista instrumental acelerada en bucle de estilo chiptune / rock de 8 bits con ritmo elevado (BPM alto).
* **Efectos de Sonido (SFX):**
  * Sonido de aceleracion al presionar el boton de Turbo-Dash.
  * Sonido de impacto y ruptura de hielo.
  * Sonido de alerta al activar el Modo Caña.
  * Sonido de impacto final al ser alcanzado por la avalancha.

---

## 6. CRONOGRAMA DE TRABAJO (7 DIAS - SOLO DEV)

### Dia 1: Prototipado Base
* Creación del proyecto 2D en Unity.
* Programacion del movimiento constante del jugador y seguimiento de camara.
* Implementación de la mecanica de entrada del Turbo-Dash.

### Dia 2: Sistema de Terreno y Generacion
* Creación del sistema de Object Pooling para tramos de terreno.
* Generacion aleatoria de bloques de hielo, rocas y precipicios.

### Dia 3: Logica de Colisiones y Avalancha
* Configuracion de los tipos de obstaculos y sus valores de frenado.
* Implementacion de la avalancha y calculo de distancia respecto al jugador.
* Programacion de las condiciones de Game Over.

### Dia 4: Modo Caña e Interfaz
* Creación de la barra de energia y su logica de llenado.
* Programacion del estado de invencibilidad y multiplicadores de puntos.
* Montaje de la interfaz de usuario (Puntuación actual, Barra de Caña, Indicador de velocidad).

### Dia 5: Integracion de Arte y Efectos
* Sustitucion de marcadores de posicion por sprites finales.
* Configuración del Trail Renderer y los sistemas de particulas.
* Implementacion del script de temblor de camara (Camera Shake).

### Dia 6: Integracion de Audio y Pulido
* Implementación de la musica en bucle y efectos de sonido.
* Calibracion de la curva de dificultad (incremento progresivo de la velocidad del juego).
* Creación del menu principal y pantalla de reinicio.

### Dia 7: Pruebas, Optimizacion y Publicacion
* Pruebas de rendimiento y correccion de errores de colision.
* Exportacion de la version final (Build WebGL / PC).
* Carga del proyecto en la plataforma de la Game Jam (itch.io).