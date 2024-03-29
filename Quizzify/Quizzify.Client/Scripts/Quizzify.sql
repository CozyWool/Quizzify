﻿CREATE TABLE SecretQuestions 
(
    secret_q_id SERIAL PRIMARY KEY,
    question_text TEXT NOT NULL
);

CREATE TABLE Users
(
    user_id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    login VARCHAR(20) NOT NULL UNIQUE,
    password_hash VARCHAR(256) NOT NULL,
    email VARCHAR(50) NOT NULL UNIQUE,
    selected_secret_question_id INTEGER,
    secret_answer_hash VARCHAR(256) NOT NULL, 
    TwoFA_auth_method TEXT,
    Google_Authorization BYTEA,
    CONSTRAINT secret_question_fk FOREIGN KEY(selected_secret_question_id) REFERENCES SecretQuestions(secret_q_id) ON DELETE SET NULL
);

CREATE TABLE Players
(
    player_id SERIAL PRIMARY KEY,
    user_id UUID UNIQUE NOT NULL,
    nickname VARCHAR(20) NOT NULL,
    user_profile_picture BYTEA, 
    about TEXT,
    CONSTRAINT user_id_fk FOREIGN KEY(user_id) REFERENCES Users(user_id) ON DELETE CASCADE
);

CREATE TABLE Packages
(
    package_id SERIAL PRIMARY KEY,
    package_name VARCHAR(50) NOT NULL,
    created_at DATE DEFAULT CURRENT_DATE,
    difficulty INTEGER NOT NULL CHECK (difficulty >= 1 AND difficulty <= 10)
);

CREATE TABLE Rounds
(
    round_id SERIAL PRIMARY KEY,
    package_id INTEGER NOT NULL,
    name VARCHAR(255) NOT NULL,
    round_type VARCHAR(7) NOT NULL CHECK(round_type ILIKE 'Обычный' or round_type ILIKE 'Финал'),
    CONSTRAINT rounds_package_id_fk FOREIGN KEY (package_id) REFERENCES Packages(package_id) ON DELETE CASCADE
);

CREATE TABLE Questions
(
    question_id SERIAL PRIMARY KEY,
    round_id INTEGER NOT NULL,
    question_text TEXT,
    question_theme VARCHAR(15),
    question_image_url BYTEA,
    question_cost INTEGER NOT NULL CHECK(question_cost >= 1),
    question_comment TEXT,
    answer_text TEXT,
    answer_image_url BYTEA,
    CONSTRAINT questions_round_id_fk FOREIGN KEY (round_id) REFERENCES Rounds(round_id) ON DELETE CASCADE,
    CONSTRAINT at_least_one_question CHECK(question_text IS NOT NULL OR question_image_url IS NOT NULL),
    CONSTRAINT at_least_one_answer CHECK(answer_text IS NOT NULL OR answer_image_url IS NOT NULL)
);
