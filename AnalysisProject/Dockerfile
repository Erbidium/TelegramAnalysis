FROM python:3.9-slim-buster
RUN apt-get update && apt-get install -y \
    build-essential \
    libssl-dev \
    libffi-dev \
    unixodbc \
    unixodbc-dev \
    wget \
    && apt-get clean \
    && rm -rf /var/lib/apt/lists/*

RUN pip install pipenv
WORKDIR /app
COPY . /app
COPY requirements.txt /app/
RUN pip install --no-cache-dir -r requirements.txt
COPY navec_hudlit_v1_12B_500K_300d_100q.tar /app/navec_hudlit_v1_12B_500K_300d_100q.tar
EXPOSE 5000
CMD ["python", "app.py"]
