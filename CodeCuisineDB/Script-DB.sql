CREATE DATABASE codecuisine;
USE codecuisine;

CREATE TABLE adminData(
	id_admin INT AUTO_INCREMENT,
	email VARCHAR(45),
    password VARCHAR(56),
    PRIMARY KEY(id_admin)
);

CREATE TABLE userData(
	id_user INT AUTO_INCREMENT,
	email VARCHAR(45),
    user_name VARCHAR(45),
    PRIMARY KEY(id_user)
);

CREATE TABLE playerData(
	id_player INT AUTO_INCREMENT,
	id_user INT,
    player_name VARCHAR(12),
    PRIMARY KEY(id_player),
		FOREIGN KEY (id_user) REFERENCES userData(id_user)
);

CREATE TABLE levelData(
	id_level INT AUTO_INCREMENT,
    level_number INT,
    duration FLOAT,
    PRIMARY KEY(id_level)
);

CREATE TABLE levelProgress(
	id_progress INT AUTO_INCREMENT,
    id_level INT,
    id_player INT,
    score INT,
    start_time DATETIME,
    finish_time DATETIME,
    PRIMARY KEY(id_progress),
		FOREIGN KEY (id_player) REFERENCES playerData(id_player),
		FOREIGN KEY (id_level) REFERENCES levelData(id_level)	
);

CREATE TABLE session(
	id_session INT AUTO_INCREMENT,
    id_player INT,
    date_session DATETIME,
    PRIMARY KEY(id_session),
    	FOREIGN KEY (id_player) REFERENCES playerData(id_player)
);

DELIMITER //
CREATE PROCEDURE validateUser(
    IN email VARCHAR(45),
    IN player_name VARCHAR(12)
)
BEGIN
    SELECT COUNT(*) INTO @user_count FROM userData WHERE email = email;
    
    IF @user_count = 0 THEN
        INSERT INTO userData (user_name, email) VALUES (player_name, email);
        
        SELECT COUNT(*) INTO @player_count FROM playerData WHERE player_name = player_name;
        
        IF @player_count = 0 THEN
            INSERT INTO playerData (player_name, id_user) 
            VALUES (player_name, (SELECT id_user FROM userData WHERE email = email));
        END IF;
    END IF;
END //
DELIMITER ;



DELIMITER //
CREATE PROCEDURE getStats(IN playerName VARCHAR(255))
BEGIN
    DECLARE playerId INT;
    
    -- Get id_player
    SELECT id_player INTO playerId FROM playerData WHERE player_name = playerName;
    
    -- Get time played
    SELECT (SUM(finish_time - start_time)) INTO @time_played 
    FROM levelProgress WHERE id_player = playerId;
    
    -- Get total matches completed
    SELECT (COUNT(*)) INTO @total_levels 
    FROM levelProgress WHERE id_player = playerId;
    
    -- Get total levels completed
    SELECT (COUNT(DISTINCT id_level)) INTO @level_count 
    FROM levelProgress WHERE id_player = playerId;
    
    -- Get total score
    SELECT (SUM(score)) INTO @total_score 
    FROM levelProgress WHERE id_player = playerId;
    
    -- Get player ranking
    SELECT (COUNT(*) + 1) INTO @player_rank FROM (
        SELECT pd.player_name, SUM(lp.score) as total_score 
        FROM levelProgress lp
        INNER JOIN playerData pd ON lp.id_player = pd.id_player
        GROUP BY lp.id_player, pd.player_name
        HAVING total_score > (
            SELECT SUM(lp2.score) as total_score
            FROM levelProgress lp2
            INNER JOIN playerData pd2 ON lp2.id_player = pd2.id_player
            WHERE pd2.player_name = playerName
            GROUP BY pd2.player_name
        )
    ) as ranks;
    
    -- Send all results
    SELECT @time_played AS time_played,
           @total_levels AS total_levels,
           @level_count AS level_count,
           @total_score AS total_score,
           @player_rank AS player_rank;
END//
DELIMITER ;



DELIMITER //
CREATE PROCEDURE createLevelProgress(
    IN _player_name VARCHAR(12),
    IN level_id INT,
    IN score INT,
    IN start_time DATETIME,
    IN finish_time DATETIME
)
BEGIN
    DECLARE player_count INT;
    DECLARE player_id INT;
    
    -- Count players
    SELECT COUNT(*) INTO player_count FROM playerData WHERE player_name = _player_name;
    
    -- Verify if the player exists
    IF player_count > 1 THEN
        SELECT 'Multiple players found' AS message;
    ELSEIF player_count = 0 THEN
        SELECT 'Player not found' AS message;
    ELSE
        -- Get id_player
        SELECT id_player INTO player_id FROM playerData WHERE player_name = player_name LIMIT 1;
        
        -- Insert player's level progress
        INSERT INTO levelProgress (id_player, id_level, score, start_time, finish_time) 
        VALUES (player_id, level_id, score, start_time, finish_time);
        
        SELECT 'Level progress created' AS message;
    END IF;
END//
DELIMITER ;


DELIMITER //
CREATE PROCEDURE createPlayer(
    IN _player_name VARCHAR(12)
)
BEGIN
    DECLARE id_user INT DEFAULT NULL;
    DECLARE player_count INT;
    -- Verify if the player exists
    SELECT COUNT(*) INTO player_count FROM playerData WHERE player_name = _player_name;
    IF player_count = 0 THEN
        -- Insert the new player if it does not exist and get the message
        INSERT INTO playerData (player_name, id_user) VALUES (_player_name, id_user);
        SELECT 'Player created' AS message;
    ELSE
        -- Get the message that the player already exists
        SELECT 'Player already exists' AS message;
    END IF;
END//
DELIMITER ;


