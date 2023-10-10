# Kata

## **Objectifs**

- Montrer ce que vous savez faire dans un temps raisonnable et voir comment vous codez dans la vrai vie.
- Faire une PR sur la branche **main** afin que nous puissions te faire des commentaires.
- Il s’agit d’un projet que devrait bientôt partir en **production** en ce qui concerne à la qualité du code.

Dans le cadre de ce projet, nous souhaitons avoir une Api :

1. Je veux que mon sensor récupère la température provenant du composant `TemperatureSensor` (renvoi la température en degrés Celsius).
2. Je veux que l'état de mon Sensor soit à "HOT" lorsque la température captée est supérieure ou égale a 35 °C.
3. Je veux l'état de mon Sensor soit à "COLD" lorsque la température captée est inférieur a 22°C.
4. Je veux l'état de mon Sensor soit à "WARM" lorsque la température captée est entre 22°C et inférieur à 35 °C.
5. Je veux récupérer l'historique des quinze dernières demandes des températures.
6. Je veux pouvoir redéfinir les limites pour "HOT", "COLD", "WARM".

## **Stack Obligatoire**

- .NET Core 6 ou supérieur
- SQL Lite
- Docker
