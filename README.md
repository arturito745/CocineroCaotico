**Documentación del Proyecto "Cocinero Caótico"**  
**Autores**: Ricardo Guerrero y Juan José Builes  

---

**Tabla de Contenidos**  
1. [Descripción del Juego](#descripción-del-juego)  
2. [Requisitos del Sistema](#requisitos-del-sistema)  
3. [Instrucciones para Ejecutar](#instrucciones-para-ejecutar)  
4. [Recursos de Terceros](#recursos-de-terceros)  
5. [Estado Actual del Proyecto](#estado-actual-del-proyecto)  
6. [Licencia](#licencia)  

---

**Descripción del Juego**  
Eres un chef en una cocina mágica donde los ingredientes tienen vida propia. Debes atraparlos y combinarlos en la olla siguiendo recetas específicas antes de que se acabe el tiempo.  

Mecánicas principales:  
- Movimiento del chef con teclas WASD  
- Ingredientes con comportamientos únicos (ej: tomate que rueda)  
- Sistema de recetas y temporizador por nivel  

---

**Requisitos del Sistema**  
- Sistema operativo: Windows 10/11  
- Software:  
  - Para ejecutar el juego: Solo se necesita el archivo descargable  
  - Para modificar el código:  
    - Unity Hub con versión exacta: Unity 6000.0.43f1  
    - Visual Studio (opcional)  

---

**Instrucciones para Ejecutar**  
**Versión Ejecutable (sin código fuente)**  
1. Descargar:  
   - Accede al enlace de Google Drive (https://drive.google.com/drive/folders/1P01Lpgc7NVuN1eQSHs28KESXESnkGRP2) y descarga la carpeta Ejecutable_Cocinero  
2. Extraer:  
   - Haz clic derecho en la carpeta y selecciona "Extraer todo"  
3. Ejecutar:  
   - Abre la carpeta extraída y haz doble clic en Cocinero_Caotico.exe  

Nota: Si Windows bloquea el archivo, haz clic en "Más información" y luego en "Ejecutar de todas formas"  

---

**Recursos de Terceros**  

| Recurso               | Enlace | Licencia | Uso en el Proyecto |  
|-----------------------|--------|----------|--------------------|  
| Modern 2D Kitchen | https://assetstore.unity.com/packages/2d/environments/modern-2d-platformer-kitchen-furniture-275205 | Unity EULA | Escenario principal |  
| Food Icons Pack | https://assetstore.unity.com/packages/2d/gui/icons/food-icons-pack-70018 | Unity EULA | Iconos de ingredientes |  
| 2D Beginner Adventure Game | https://assetstore.unity.com/packages/essentials/tutorial-projects/unity-learn-2d-beginner-adventure-game-complete-project-urp-271239 | CC-BY 4.0 | Mecánicas base |  
| House Interior Tileset | https://assetstore.unity.com/packages/2d/environments/house-interior-tileset-32x32-lite-307715 | Unity EULA | Decoración adicional |  

---

**Estado Actual del Proyecto**  

**Funcionalidades Implementadas**  
- Movimiento del jugador con WASD  
- IA básica para el tomate (rodar, evadir al jugador)  
- Sistema de recolección y arrastre de ingredientes  
- Olla que valida ingredientes agregados (sin verificación final de receta)  

**Bugs Conocidos**  
- El tomate a veces no entra correctamente en la olla  
- Detección de colisiones inconsistente en la clase Pot  

**Próximos Pasos**  
1. Implementar IA para la zanahoria (esconderse) y el huevo (saltar)  
2. Añadir pantallas de victoria/derrota  
3. Mejorar feedback visual al cocinar ingredientes  

