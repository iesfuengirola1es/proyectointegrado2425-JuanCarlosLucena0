# Propuesta del Proyecto

## Introducción

En la actualidad, muchos juegos carecen de una experiencia compacta que combine simplicidad, desafío estratégico y rejugabilidad. Halaman Tower surge para atender esta necesidad, ofreciendo un RPG por turnos con mecánicas de rogue-like, donde el jugador se enfrenta a una serie ininterrumpida de enemigos, eligiendo cómo mejorar sus estadísticas y busca maximizar su puntuación.

Lo que ofrece este videojuego brindar una experiencia rápida, estratégica y competitiva, ideal para quienes buscan partidas dinámicas pero con profundidad táctica. Además, incorpora un sistema de registro en línea que fomenta la competitividad al permitir a los jugadores comparar sus resultados, integrando de forma innovadora componentes locales y en línea en un formato accesible y rejugable.

## Objetivos

Halaman Tower consistirá principalmente de un videojuego “rogue-like” por turnos, esto significa que será un juego donde cada partida que el jugador realice, será una experiencia única, no solo comparada a la anterior, si no también a la que tienen otros jugadores.

Los enemigos aparecerán de uno en uno de forma aleatoria, donde al vencerlos, ganaremos experiencia, subiendo de nivel nos haremos más fuertes, lo cual será necesario ya que cada vez que venzamos un enemigo, subiremos un piso de la torre, haciendo que los enemigos sean más fuertes, pero a su vez, den más experiencia.

Para derrotar a estos enemigos, usaremos nuestras habilidades, las cuales se basarán principalmente en 3 tipos, las que afectan a la vida de forma porcentual, las que afectan a una cantidad fija de vida y las potencia de ataque, en las cuales, estas se verán influenciadas por nuestro atributo de ataque, al igual que por el atributo de defensa del rival.

Cada 10 pisos nos enfrentaremos a un pequeño jefe, el cual supone ser más fuerte que los enemigos anteriores a cambio de una mayor cantidad de experiencia, sumado a esto y para una mayor variedad, por cada piso múltiplo de 5, obtendremos un objeto con el que poder aumentar un atributo a nuestra elección, dando vía libre a la personalización según nuestro estilo de juego, los enemigos tendrán atributos aleatorios basados en su nivel, es decir, es posible que en una partida, el piso 20 tenga un enemigo con un daño muy ligero, pero que sin embargo aguante mucho, mientras que en otra, podría pasar todo lo contrario.

Al igual que los enemigos subirán aleatoriamente sus estadísticas, nosotros también, forzándonos a no poder depender de los mismos objetos que suban nuestra estadísticas cada vez.

Por último, el juego tendrá una conexión con una base de datos en PythonAnywhere, haciendo que se registre nuestra puntuación como usuario, la cual se podrá ver en una “leaderboard” dentro del juego.

## Entorno Tecnológico
## Tecnologías empleadas:

### Lenguajes de Programación

- **C#:** Utilizado para la lógica y mecánicas del juego en Unity, el motor principal de desarrollo.

- **Python:** Usado para el desarrollo del servidor online que registra y consulta las puntuaciones.

### Frameworks

- **Flask:** Framework ligero para construir la API que conectará el juego con la base de datos en el servidor.

### Motores

- **Unity:** Motor de desarrollo de videojuegos

### Gestores de Bases de Datos

- **MySQL:** Base de datos más robusta que nos permite manejar un volumen mayor de datos que opciones como SQLite

### Recursos Web

- **PythonAnywhere:** Servicio para alojar el servidor y la base de datos en línea.

- **Suno:** Servicio de creación de canciones.

- **MyEdit:** Servicio de creación y modificación de imágenes y audio.

- **Piskel:** Servicio para creación de “sprites”

## Recursos software y harware
## Software

- **Visual Studio Code:** Editor de código para escribir scripts en C# y editar configuraciones.

- **Unity Editor:** Entorno de desarrollo integrado (IDE) para crear el juego.

### Software online:

- **PythonAnywhere:** Servicio web utilizado para alojar el servidor y la base de datos.

- **Piskel:** Herramienta en línea para la creación de sprites y pixel art.

- **MyEdit:** Plataforma web para edición de imágenes y audio.

Suno: Servicio web utilizado para generar música.

## Referencias

- **PokeRogue:** “Rogue-like” de pokémon

- **Octopath:** Juego de rol por turnos