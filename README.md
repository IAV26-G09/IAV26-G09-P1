# Hamelin - Base
Prototipo de videojuego actualizado a la versión LTS de Unity llamada **Unity 6000.0.62f2** , diseñado para servir como punto de partida en algunas prácticas.

Consiste en un entorno virtual 3D que representa el pueblo de Hamelín, un personaje controlable por el jugador que es el flautista de Hamelín, un perro compañero y un montón de ratas preparadas para controlarse con IA.

## Estados de los agentes

- Ratas:
```mermaid
graph TD;
  MERODEO<-->HIPNOSIS;
```

- Perro:
```mermaid
graph TD;
  SEGUIR_FLAUTISTA<-->HUIR_RATAS;
```

- Flautista:
```mermaid
graph TD;
  TOCAR_FLAUTA<-->NO_TOCAR_FLAUTA;
```
  

## Licencia
Federico Peinado, autor de la documentación, código y recursos de este trabajo, concedo permiso permanente a los alumnos de la Facultad de Informática de la Universidad Complutense de Madrid para utilizar este material, con sus comentarios y evaluaciones, con fines educativos o de investigación; ya sea para obtener datos agregados de forma anónima como para utilizarlo total o parcialmente reconociendo expresamente mi autoría.

## Referencias
A continuación se detallan todas las referencias bibliográficas, lúdicas o de otro tipo utilizdas para realizar este prototipo. Los recursos de terceros que se han utilizados son de uso público.

Lousberg, K. (s. f.). [*Kaykit animations*](https://kaylousberg.itch.io/kaykit-animations)

Lousberg, K. (s. f.). [*Kaykit dungeon*](https://kaylousberg.itch.io/kaykit-dungeon)

Lousberg, K. (s. f.). [*Kaykit medieval builder pack*](https://kaylousberg.itch.io/kaykit-medieval-builder-pack)

Millington, I. (2019). *AI for games* (3rd ed.). CRC Press.
