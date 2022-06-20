# Сбор статистики по полу и возрасту
Этот проект предстовляет из себя десктопное Windows приложение, которое распознает пол и возраст. На основе этих данных программа строит график, который далее можно сохранить в формате Jpeg.

# Суть нашей работы
Создать приложение, которое получает изображение c видеокамеры в реальном времени, обрабатывает его, передает обработанные данные в готовую стороннюю нейросеть. Далее по результату работы нейронной сети, строиться график, который отражает, какое количество, какого пола, людей попало на камеру. Сбор данных производиться по определенному интервалу, который можно настроить.

# Модели нейронных сетей
Модели для распознавания возраста и пола представленны в виде модели caffe, которые скачаны с этого сайта:
  https://talhassner.github.io/home/publication/2015_CVPR
Модель для распознавания лица представлена в виде каскадного классификатора Хаары, взятого с этого сайта:
  https://translated.turbopages.org/proxy_u/en-ru.ru.b61985c7-62b097ac-445d81d0-74722d776562/https/github.com/opencv/opencv/tree/master/data/haarcascades

